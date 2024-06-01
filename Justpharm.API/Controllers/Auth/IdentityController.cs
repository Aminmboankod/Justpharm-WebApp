using Justpharm.Library.DTO;
using log4net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Justpharm.API.Controllers.Auth;

[Route("api/accounts")]
[ApiController]
public class IdentityController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IEmailSender _emailSender;
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection configurationSection;
    private readonly ILog Logger = LogManager.GetLogger(typeof(IdentityController));

    public IdentityController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager, IEmailSender emailSender, IConfiguration configuration)
    {
        _userManager = userManager;
        signInManager = signinManager;
        _emailSender = emailSender;
        _configuration = configuration;
        //configurationSection = _configuration.GetSection("JwtSettings");
    }

    [HttpPost("login")] // POST .../api/accounts/login
    public async Task<IActionResult> Login([FromBody] UserRegisterDto user)
    {
        try
        {

            var result = await signInManager.PasswordSignInAsync(user.Email, user.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded) return BadRequest(new AuthResponseDto { IsAuthSuccessful = false, ErrorMessage = "Usuario o contraseña incorrectos" });


            Claim[]? claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
            };

            SymmetricSecurityKey? key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]!));
            SigningCredentials? creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expiry = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpiryInDays"]));

            JwtSecurityToken? token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });

        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return Problem(ex.Message);
        }
    }

    [HttpPost("register")] // POST .../api/accounts/register
    public async Task<IActionResult> Register([FromBody] UserRegisterDto user)
    {
        var result = await CreateUser(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new AuthResponseDto { ErrorMessage = errors.First() });
        }

        IdentityUser? u = await _userManager.FindByEmailAsync(user.Email);
        string token = await _userManager.GenerateEmailConfirmationTokenAsync(u);

        // Enviar email de confirmación
        await SendConfirmRegisterEmail(token, u.Email!, u.Id);
        return StatusCode(201);
    }

    [HttpDelete("delete/{email}")] // DELETE .../api/accounts/delete/{email}
    public async Task<IActionResult> DeleteUser(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return NotFound();
        }
        await _userManager.DeleteAsync(user);
        return Ok();
    }

    [HttpPost("change-password")] // POST .../api/accounts/change-password
    public async Task<IActionResult> ChangePassword([FromBody] UserRegisterDto user)
    {
        var u = await _userManager.FindByEmailAsync(user.Email);
        if (u == null)
        {
            return NotFound();
        }
        var result = await _userManager.ChangePasswordAsync(u, u.PasswordHash, user.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new AuthResponseDto { ErrorMessage = errors.First() });
        }
        return Ok();
    }

    [HttpPost("forgot-password")] // POST .../api/accounts/forgot-password
    public async Task<IActionResult> ForgotPassword([FromBody] UserRegisterDto user)
    {
        var u = await _userManager.FindByEmailAsync(user.Email);
        if (u == null || !(await _userManager.IsEmailConfirmedAsync(u)))
        {
            return NotFound();
        }
        var token = await _userManager.GeneratePasswordResetTokenAsync(u);
        await SendResetPassEmail(token, user.Email);
        return Ok(token);
    }

    [HttpPost("logout")] // POST .../api/accounts/logout
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Ok();
    }


    ////////////////////////////////// METODOS PRIVADOS //////////////////////////////////////////


    private async Task<IdentityResult> CreateUser(UserRegisterDto _usuario)
    {
        try
        {
            // Primero se comprueba si el usuario existe, si no existe se crea
            var user = await _userManager.FindByEmailAsync(_usuario.Email);

            if (user == null)
            {
                var email = _usuario.Email;
                var creation = await _userManager.CreateAsync(new IdentityUser { UserName = _usuario.Email, NormalizedUserName = _usuario.Email, Email = _usuario.Email, NormalizedEmail = _usuario.Email, EmailConfirmed = true });

                if (!creation.Succeeded)
                {
                    var u = await _userManager.FindByEmailAsync(_usuario.Email);
                    Logger.Error("Error al crear el usuario: " + u.Email);
                    foreach (var error in creation.Errors)
                    {
                        Logger.Error("Error: " + error.Description);
                    }
                    await _userManager.DeleteAsync(u);
                }
                else
                {
                    await _userManager.AddPasswordAsync(_userManager.FindByEmailAsync(_usuario.Email).Result!, _usuario.Password);
                    Logger.Info("Usuario creado correctamente");
                }
                return creation;
            }
            else return IdentityResult.Failed(new IdentityError { Code = "UserExists", Description = "El usuario ya existe" });
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return IdentityResult.Failed(new IdentityError { Code = "Error", Description = ex.Message });
        }

    }

    private async Task SendResetPassEmail(string code, string email)
    {
        try
        {
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                email,
                "Recuperación de contraseña",
                $"Por favor, cambie su contraseña <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>pulsando aquí</a>.");

        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }

    private async Task SendConfirmRegisterEmail(string code, string email, string userId)
    {
        try
        {
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirme su correo electrónico",
                $"Por favor, confirme su cuenta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>pulsando aquí</a>.");

        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }

}
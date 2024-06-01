using Justpharm.Library.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using log4net;

namespace Justpharm.API.Controllers.Tratamiento;

[Route("api/tratamiento")]
[ApiController]
public class TratamientoController : Controller
{
    private readonly ILog Logger = LogManager.GetLogger(typeof(TratamientoController));
    private readonly UserManager<IdentityUser> _userManager;
    public TratamientoController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("nuevo")]
    public async Task<IActionResult> NuevoTratamiento([FromBody] NuevoTratamientoDto nuevoTratam)
    {
        try
        {
            IdentityUser? user = GetUsuario(nuevoTratam.UserId);
            if (user != null)
            {
                Logger.Info($"El usuario {nuevoTratam.UserId} va a crear un nuevo tratamiento");
                return Ok();
            }

            Logger.Info($"El usuario {nuevoTratam.UserId} no existe.");
            return NotFound();
        }
        catch (Exception ex)
        {
            Logger.Error($"El siguiente error ha impedido crear el tratamiento: {ex.Message}");
            return BadRequest();
        }
    }

    [HttpGet("mis-tratamientos/{userid}")]
    public async Task<IActionResult> MisTratamientos(string userid)
    {
        try
        {
            IdentityUser? user = GetUsuario(userid);
            var Tratamientos = 1;
            if (user != null)
            {
                // Db.All<Tratamiento>(t = t.UserId == user.Id).ToList();
                return Ok();
            }
            Logger.Info($"El usuario no existe.");
            return NotFound();
            
        }
        catch (Exception ex)
        {
            Logger.Error($"El siguiente error ha impedido obtener tratamientos: {ex.Message}");
            return BadRequest();
        }
    }

    [HttpPost("editar")]
    public async Task<IActionResult> EditarTratamiento([FromBody] EditarTratamientoDto tratEdit)
    {
        try
        {
            IdentityUser? user = GetUsuario(tratEdit.UserId);

            if (user != null)
            {
                return Ok();
            }
            
            Logger.Error($"No ha sido posible editar el tratamiento porque el usuario no existe");
            return NotFound();
        }
        catch (Exception ex)
        {
            Logger.Error($"El siguiente error ha impedido editar un tratamiento: {ex.Message}");
            return BadRequest();
        }
    }

    [HttpPost("eliminar")]
    public async Task<IActionResult> EliminarTratamiento([FromBody] EliminarTratamientoDto tratam)
    {
        try
        {
            IdentityUser? user = GetUsuario(tratam.UserId);
            if (user != null)
            {
                // Db.Delete<Tratamiento>(tratam);
                Logger.Info($"El usuario {user.Email} ha eliminado satisfactoriamente el tratamiento: $");
                return Ok();
            }
            Logger.Error($"No ha sido posible eliminar el tratamiento porque el usuario no existe");
            return NotFound();
        }
        catch (Exception ex)
        {
            Logger.Error($"El siguiente error ha impedido eliminar un tratamiento: {ex.Message}");
            return BadRequest();
        }
    }
    
    
    ///////// FUNCIONES PRIVADAS ///////////////


    private IdentityUser? GetUsuario(string userId)
    {
        return _userManager.Users.First(u => u.Id == userId);
    }
}
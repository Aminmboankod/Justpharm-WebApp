
using Justpharm.Library.Models;
using Justpharm.Library.Repository;
using log4net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Justpharm.API.Controllers.Pacientes;

[Route("api/paciente")]
[ApiController]
public class PacienteController : Controller
{
    private readonly ILog Logger = LogManager.GetLogger(typeof(PacienteController));
    private readonly UserManager<IdentityUser> _userManager;
    private DbQry Qry;

    private JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = true
    };
    public PacienteController(UserManager<IdentityUser> userManager, DbQry qry)
    {
        _userManager = userManager;
        Qry = qry;
    }

    [HttpGet("get/{userEmail}")]
    public async Task<IActionResult> GetPaciente(string userEmail)
    {
        try
        {
            IdentityUser? user = await _userManager.FindByEmailAsync(userEmail);
            Paciente? paciente = Qry.All<Paciente>(p => p.UserId == user!.Id).FirstOrDefault();
            return Content(JsonSerializer.Serialize(paciente, _jsonOptions), "application/json");
        }
        catch (Exception e)
        {
            Logger.Error($"El siguiente error ha impedido obtener el paciente: {e.Message}");
            return BadRequest();
        }
    }

    [HttpPost("nuevo")]
    public async Task<IActionResult> NuevoPaciente([FromBody] Library.Models.Paciente nuevoPaciente)
    {
        try
        {
            IdentityUser? user = await _userManager.FindByIdAsync(nuevoPaciente.UserId);
            if (user != null)
            {
                Qry.Insert(nuevoPaciente);
                return Ok();
            }

            Logger.Info($"El usuario {nuevoPaciente.UserId} no existe.");
            return NotFound();
        }
        catch (Exception ex)
        {
            Logger.Error($"El siguiente error ha impedido crear el paciente: {ex.Message}");
            return BadRequest();
        }
    }
}
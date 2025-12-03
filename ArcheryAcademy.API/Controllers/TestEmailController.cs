using ArcheryAcademy.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestEmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public TestEmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendTestEmail(string to)
    {
        await _emailService.SendAsync(
            to,
            "Correo de prueba",
            "<h1>¡Hola! Tu correo está funcionando 😄</h1>"
        );

        return Ok(new { message = "Correo enviado correctamente" });
    }
}
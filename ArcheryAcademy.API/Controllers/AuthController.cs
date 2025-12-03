using ArcheryAcademy.Application.DTOs.AuthDto;
using ArcheryAcademy.Application.UseCases.AuthUseCases.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var command = new LoginCommand(dto);
        var result = await mediator.Send(command);

        if (result == null)
            return Unauthorized(new { message = "Credenciales inválidas" });

        return Ok(result);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var command = new RegisterUserCommand(dto);
        var result = await mediator.Send(command);

        if (result == null)
        {
            return BadRequest(new { message = "El correo electrónico ya está registrado." });
        }

        return Ok(result);
    }
}
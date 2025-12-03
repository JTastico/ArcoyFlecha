using ArcheryAcademy.Application.DTOs.UserDto;
using ArcheryAcademy.Application.UseCases.UserUseCases.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    // PUT: api/users/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateDto dto)
    {
        // Validación básica de integridad
        if (id != dto.Id)
            return BadRequest(new { message = "El ID de la URL no coincide con el del cuerpo." });

        try
        {
            var command = new UpdateUserCommand(id, dto);
            var result = await mediator.Send(command);

            if (!result) return NotFound(new { message = $"Usuario con ID {id} no encontrado." });

            // 204 No Content es el estándar para actualizaciones exitosas
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            // Captura: "El rol no existe"
            return BadRequest(new { message = ex.Message });
        }
    }
    
    
    [HttpPatch("status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UserStatusUpdateDto dto)
    {
        var command = new UpdateUserStatusCommand(dto);
        var result = await mediator.Send(command);

        if (!result)
            return NotFound(new { message = $"No se encontró al usuario '{dto.FirstName} {dto.LastName}'." });

        return NoContent();
    }
}
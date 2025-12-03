using ArcheryAcademy.Application.Common;
using ArcheryAcademy.Application.DTOs.UserDto;
using ArcheryAcademy.Application.UseCases.UserUseCases.Command;
using ArcheryAcademy.Application.UseCases.UserUseCases.Queries;
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
    //Craar usuario
    [HttpPost]
    public async Task<ActionResult<UserReadDto>> Create([FromBody] UserCreateDto dto)
    {
        try
        {
            var result = await mediator.Send(new CreateUserCommand(dto));
            return Ok(result); 
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    // GET: api/Users?page=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<PagedResult<UserReadDto>>> GetAll(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var query = new GetAllUsersQuery 
        { 
            Page = page, 
            PageSize = pageSize 
        };
        var result = await mediator.Send(query);

        return Ok(result);
    }
    
    // GET: api/Users/{id}
    [HttpGet("{id:guid}")] // Importante: Restricción :guid
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await mediator.Send(query);

        if (result == null)
            return NotFound(new { message = $"No se encontró el usuario con ID {id}." });

        return Ok(result);
    }
}
using ArcheryAcademy.Application.DTOs.UserRoleDto;
using ArcheryAcademy.Application.UseCases.UserRoleUseCases.Command;
using ArcheryAcademy.Application.UseCases.UserRoleUseCases.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserRoleController(IMediator mediator, IMapper mapper) : ControllerBase
{
    // GET: api/userrole
    [HttpGet]
    public async Task<ActionResult<List<UserRoleReadDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllUserRolesQuery());
        return Ok(result);
    }

    // GET by id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetUserRoleByIdQuery(id));

        if (result == null)
            return NotFound(new { message = $"UserRole with ID {id} not found." });

        return Ok(result);
    }

    // POST (Crear)
    [HttpPost]
    public async Task<IActionResult> CreateUserRole([FromBody] UserRoleCreateDto dto)
    {
        var command = new CreateUserRoleCommand(dto);
        var createdEntity = await mediator.Send(command);

        var resultDto = mapper.Map<UserRoleReadDto>(createdEntity);

        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    // PUT (Actualizar)
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUserRole(Guid id, [FromBody] UserRoleUpdateDto dto)
    {
        var command = new UpdateUserRoleCommand(id, dto);
        var updatedEntity = await mediator.Send(command);

        if (updatedEntity is null)
            return NotFound($"UserRole with ID {id} not found.");

        var resultDto = mapper.Map<UserRoleReadDto>(updatedEntity);
        return Ok(resultDto);
    }

    // DELETE
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteUserRoleCommand(id));

        if (!result)
            return NotFound($"UserRole with ID {id} was not found.");

        return NoContent();
    }
}

using ArcheryAcademy.Application.Dtos.PaymentDto;
using ArcheryAcademy.Application.DTOs.RolDto;
using ArcheryAcademy.Application.UseCases.PlanUseCases.Queries;
using ArcheryAcademy.Application.UseCases.RoleUseCases.Commands;
using ArcheryAcademy.Application.UseCases.RoleUseCases.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleControler(IMediator mediator, IMapper mapper): ControllerBase
{
    // GET all
    [HttpGet]
    public async Task<ActionResult<List<RolReadDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllRoleQuery());
        return Ok(result);
    }
    
    // GET by id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetRoleByIdQuery(id));
        
        if (result == null)
            return NotFound(new { message = $"Payment with ID {id} not found." });

        return Ok(result);
    }
    
    // POST (Crear)
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RolCreateDto dto)
    {
        var command = new CreateRoleCommand(dto);
        
        var createdEntity = await mediator.Send(command); 
        
        var result = mapper.Map<RolReadDto>(createdEntity);
        
        // 201 CreatedAtAction con la URI y el DTO
        return Ok(result);
    }
    
    //update
    [HttpPut("{id:guid}")] 
    public async Task<IActionResult> UpdateRol(Guid id, [FromBody] RolUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest("The ID in the route and the body must match.");

        var command = new UpdateRoleCommand( id, dto);
        var result = await mediator.Send(command);

        if (result is null)
            return NotFound($"Role with ID {id} not found.");

        return Ok(result);
    }
    
    // DELETE
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteRoleCommand(id));

        // El Command devuelve 'true' si se eliminó, 'false' si no se encontró
        if (!result)
            return NotFound($"Payment with ID {id} was not found.");

        return NoContent(); // 204 No Content para eliminaciones exitosas
    }
}
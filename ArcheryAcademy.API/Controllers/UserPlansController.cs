using ArcheryAcademy.Application.DTOs.UserPlanDto;
using ArcheryAcademy.Application.UseCases.UserPlanUseCases.Commands;
using ArcheryAcademy.Application.UseCases.UserPlanUseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserPlansController(IMediator mediator) : ControllerBase
{
    // GET: api/userplans
    [HttpGet]
    public async Task<ActionResult<List<UserPlanReadDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllUserPlansQuery());
        return Ok(result);
    }

    // GET by id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetUserPlanByIdQuery(id));
        if (result == null)
            return NotFound(new { message = $"UserPlan with ID {id} not found." });

        return Ok(result);
    }

    // post 
    [HttpPost]
    public async Task<IActionResult> CreateUserPlan([FromBody] UserPlanCreateDto dto)
    {
        var command = new CreateUserPlanCommand(dto);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    //update
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUserPlan(Guid id, [FromBody] UserPlanUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest("The ID in the route and the body must match.");

        var command = new UpdateUserPlanCommand(id, dto);
        var result = await mediator.Send(command);

        if (result is null)
            return NotFound($"UserPlan with ID {id} not found.");

        return Ok(result);
    }
    
    //delete
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteUserPlanCommand(id));

        if (!result)
            return NotFound($"UserPlan with ID {id} was not found.");

        return NoContent();
    }
    
    // GET: api/UserPlans/report?daysThreshold=7
    [HttpGet("report")]
    public async Task<IActionResult> GetReport([FromQuery] int daysThreshold = 7)
    {
        var query = new GetUserPlanReportQuery(daysThreshold);
        var fileResult = await mediator.Send(query);

        return File(fileResult.FileContent, fileResult.ContentType, fileResult.FileName);
    }
    
    // GET: api/UserPlans/history/{userId}
    [HttpGet("history/{userId:guid}")]
    public async Task<ActionResult<List<UserPlanReadDto>>> GetHistoryByUserId(Guid userId)
    {
        var query = new GetUserPlansHistoryQuery(userId);
        var result = await mediator.Send(query);
        return Ok(result);
    }
    
    // PATCH: api/UserPlans/{id}/cancel
    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> CancelUserPlan(Guid id)
    {
        var command = new CancelUserPlanCommand(id);
        var result = await mediator.Send(command);

        if (!result)
            return NotFound(new { message = $"No se encontró el plan de usuario con ID {id}." });

        // Retornamos 204 No Content porque la acción fue exitosa pero no necesitamos devolver datos
        return NoContent(); 
    }
}
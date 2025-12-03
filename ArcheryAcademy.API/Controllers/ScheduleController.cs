using ArcheryAcademy.Application.DTOs.ScheduleDto;
using ArcheryAcademy.Application.UseCases.ScheduleUseCase.Commands;
using ArcheryAcademy.Application.UseCases.ScheduleUseCase.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController(IMediator mediator) : ControllerBase
{
    // GET: api/schedule
    [HttpGet]
    public async Task<ActionResult<List<ScheduleReadDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllSchedulesQuery());
        return Ok(result);
    }

    // GET by id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetScheduleByIdQuery(id));
        if (result == null)
            return NotFound(new { message = $"Schedule with ID {id} not found." });

        return Ok(result);
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> CreateSchedule([FromBody] ScheduleCreateDto dto)
    {
        var command = new CreateScheduleCommand(dto);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    // DELETE
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteScheduleCommand(id));

        if (!result)
            return NotFound($"Schedule with ID {id} was not found.");

        return NoContent();
    }

    // UPDATE
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateSchedule(Guid id, [FromBody] ScheduleUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest("The ID in the route and the body must match.");
        var command = new UpdateScheduleCommand(id, dto);
        var result = await mediator.Send(command);
        if (result is null)
            return NotFound($"Schedule with ID {id} not found.");
        return Ok(result);
    }
    
    // GET: api/schedule/{id}/report
    [HttpGet("{id:guid}/report")]
    public async Task<IActionResult> GetReport(Guid id)
    {

        var query = new GetScheduleReportQuery(id);
        var fileResult = await mediator.Send(query);

        if (fileResult == null)
            return NotFound(new { message = $"Schedule with ID {id} not found." });

        return File(fileResult.FileContent, fileResult.ContentType, fileResult.FileName);
    }
    // GET: api/schedule/weekly?date=2023-10-25&instructorId=GUID
    [HttpGet("weekly")]
    public async Task<ActionResult<List<ScheduleReadDto>>> GetWeeklySchedule(
        [FromQuery] DateTime? date, 
        [FromQuery] Guid? instructorId)
    {
        // Si el cliente no envía fecha, usamos "Hoy" por defecto
        var dateRef = date ?? DateTime.UtcNow;

        var query = new GetWeeklyScheduleQuery(dateRef, instructorId);
        var result = await mediator.Send(query);

        return Ok(result);
    }
}
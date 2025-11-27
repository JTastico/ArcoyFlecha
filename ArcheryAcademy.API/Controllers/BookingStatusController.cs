using ArcheryAcademy.Application.DTOs.BookingStatusDto;
using ArcheryAcademy.Application.UseCases.BookingStatusUseCases.Queries;
using ArcheryAcademy.Application.UseCases.BookingStatusUseCases.Command;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingStatusController(IMediator mediator, IMapper mapper) : ControllerBase
{
    // GET: api/bookingstatus
    [HttpGet]
    public async Task<ActionResult<List<BookingStatusReadDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllBookingStatusesQuery());
        return Ok(result);
    }

    // GET by id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await mediator.Send(new GetBookingStatusByIdQuery(id));

        if (result == null)
            return NotFound(new { message = $"BookingStatus with ID {id} not found." });

        return Ok(result);
    }

    // POST (Crear)
    [HttpPost]
    public async Task<IActionResult> CreateBookingStatus([FromBody] BookingStatusCreateDto dto)
    {
        var command = new CreateBookingStatusCommand(dto);
        var createdEntity = await mediator.Send(command);

        var resultDto = mapper.Map<BookingStatusReadDto>(createdEntity);

        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    // PUT (Actualizar)
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] BookingStatusUpdateDto dto)
    {
        var command = new UpdateBookingStatusCommand(id, dto);
        var updatedEntity = await mediator.Send(command);

        if (updatedEntity is null)
            return NotFound($"BookingStatus with ID {id} not found.");

        var resultDto = mapper.Map<BookingStatusReadDto>(updatedEntity);
        return Ok(resultDto);
    }

    // DELETE
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await mediator.Send(new DeleteBookingStatusCommand(id));

        if (!result)
            return NotFound($"BookingStatus with ID {id} was not found.");

        return NoContent();
    }
}
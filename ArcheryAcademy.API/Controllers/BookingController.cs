using ArcheryAcademy.Application.DTOs.BookingDto;
using ArcheryAcademy.Application.UseCases.BookingUseCases.Command;
using ArcheryAcademy.Application.UseCases.BookingUseCases.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController(IMediator mediator, IMapper mapper) : ControllerBase
{
    // GET: api/booking
    [HttpGet]
    public async Task<ActionResult<List<BookingReadDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllBookingsQuery());
        return Ok(result);
    }

    // GET by id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetBookingByIdQuery(id));

        if (result == null)
            return NotFound(new { message = $"Booking with ID {id} not found." });

        return Ok(result);
    }

    // POST (Crear)
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] BookingCreateDto dto)
    {
        var command = new CreateBookingCommand(dto);
        var createdEntity = await mediator.Send(command);

        var resultDto = mapper.Map<BookingReadDto>(createdEntity);

        return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
    }

    // PUT (Actualizar)
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBooking(Guid id, [FromBody] BookingUpdateDto dto)
    {
        var command = new UpdateBookingCommand(id, dto);
        var updatedEntity = await mediator.Send(command);

        if (updatedEntity is null)
            return NotFound($"Booking with ID {id} not found.");

        var resultDto = mapper.Map<BookingReadDto>(updatedEntity);
        return Ok(resultDto);
    }

    // DELETE
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteBookingCommand(id));

        if (!result)
            return NotFound($"Booking with ID {id} was not found.");

        return NoContent();
    }
    
    // GET: api/Bookings/history/{userId}
    [HttpGet("history/{userId:guid}")]
    public async Task<ActionResult<List<BookingReadDto>>> GetHistory(Guid userId)
    {
        var result = await mediator.Send(new GetBookingHistoryQuery(userId));
        return Ok(result);
    }
    
    // PATCH: api/Bookings/{id}/cancel
    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var result = await mediator.Send(new CancelBookingCommand(id));
        
        if (!result) return NotFound("Reserva no encontrada.");
        
        return NoContent();
    }
    
    // PATCH: api/Bookings/{id}/attendance
    [HttpPatch("{id:guid}/attendance")]
    public async Task<IActionResult> RegisterAttendance(Guid id)
    {
        var result = await mediator.Send(new RegisterAttendanceCommand(id));

        if (!result) return NotFound("Reserva no encontrada.");

        return NoContent();
    }

}

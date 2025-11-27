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
}
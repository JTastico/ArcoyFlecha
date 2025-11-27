using ArcheryAcademy.Application.DTOs.BookingStatusDto;
using ArcheryAcademy.Application.UseCases.BookingStatusUseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingStatusController(IMediator mediator) : ControllerBase
{
    // GET: api/bookingstatus
    [HttpGet]
    public async Task<ActionResult<List<BookingStatusReadDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllBookingStatusesQuery());
        return Ok(result);
    }
}
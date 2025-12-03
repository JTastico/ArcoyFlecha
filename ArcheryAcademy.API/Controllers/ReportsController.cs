using ArcheryAcademy.Application.DTOs.ReportDto;
using ArcheryAcademy.Application.UseCases.ReportsUsesCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArcheryAcademy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController(IMediator mediator) : ControllerBase
{
    // GET: api/Reports/bookings?from=2025-01-01&to=2025-01-31
    [HttpGet("bookings")]
    public async Task<ActionResult<BookingStatsDto>> GetBookingStats(
        [FromQuery] DateTime? from, 
        [FromQuery] DateTime? to)
    {
        // Lógica de Defaults: Si no envían parámetros, usamos el Mes Actual
        var endDate = to ?? DateTime.UtcNow;
        var startDate = from ?? new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        // Enviamos el Query a MediatR
        var query = new GetBookingStatsQuery(startDate, endDate);
        var result = await mediator.Send(query);

        return Ok(result);
    }
}
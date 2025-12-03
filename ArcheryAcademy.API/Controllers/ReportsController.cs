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
    
    // GET: api/Reports/classes?from=...&to=...
    [HttpGet("classes")]
    public async Task<ActionResult<List<ClassOccupancyDto>>> GetTopClasses(
        [FromQuery] DateTime? from, 
        [FromQuery] DateTime? to)
    {
        // Usamos la misma lógica de defaults (si es null, usa mes actual)
        var endDate = to ?? DateTime.UtcNow;
        var startDate = from ?? new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        var query = new GetTopClassesQuery(startDate, endDate);
        var result = await mediator.Send(query);

        return Ok(result);
    }
    
    // GET: api/Reports/plans?from=2023-01-01&to=2023-12-31
    [HttpGet("plans")]
    public async Task<ActionResult<PlanStatsDto>> GetPlanStats(
        [FromQuery] DateTime? from, 
        [FromQuery] DateTime? to)
    {
        // Lógica de fechas por defecto (Si es null, usamos el mes actual)
        var endDate = to ?? DateTime.UtcNow;
        var startDate = from ?? new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        var query = new GetPlanStatsQuery(startDate, endDate);
        var result = await mediator.Send(query);

        return Ok(result);
    }
    
    // GET: api/Reports/users?from=2025-01-01&to=2025-01-31
    [HttpGet("users")]
    public async Task<ActionResult<UserStatsDto>> GetUserStats(
        [FromQuery] DateTime? from, 
        [FromQuery] DateTime? to)
    {
        // Lógica de fechas por defecto (Si es null, usamos el mes actual)
        var endDate = to ?? DateTime.UtcNow;
        var startDate = from ?? new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        var query = new GetUserStatsQuery(startDate, endDate);
        var result = await mediator.Send(query);

        return Ok(result);
    }
}
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArcheryAcademy.Infrastructure.Adapters.Repositories;

public class ReportRepository(ArcheryAcademyDbContext context) : IReportRepository
{
    public async Task<(int TotalToday, int TotalRange, Dictionary<int, int> StatusCounts)> GetBookingStatsRawAsync(DateTime from, DateTime to)
    {
        var today = DateTime.UtcNow.Date;
        var query = context.Bookings.AsNoTracking().Where(b => b.CreatedAt >= from && b.CreatedAt <= to);

        var totalToday = await context.Bookings.CountAsync(b => b.CreatedAt >= today);
        var totalRange = await query.CountAsync();
        
        var statusCounts = await query
            .GroupBy(b => b.StatusId)
            .Select(g => new { Id = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Id, x => x.Count);

        return (totalToday, totalRange, statusCounts);
    }
}
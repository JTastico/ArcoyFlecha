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
    
    public async Task<List<(string Info, int Max, int Current)>> GetTopClassesRawAsync(DateTime from, DateTime to, int count = 5)
    {
        var data = await context.Schedules.AsNoTracking()
            .Where(s => s.StartTime >= from && s.StartTime <= to)
            .Select(s => new 
            {
                Info = $"{s.StartTime:dd/MM HH:mm} - {s.Instructor.FirstName}",
                // CORRECCIÓN AQUÍ: Agregamos '?? 0' para convertir int? a int
                Max = s.MaxStudents ?? 0, 
                Current = s.Bookings.Count(b => b.StatusId == 1 || b.StatusId == 2)
            })
            .OrderByDescending(x => x.Max > 0 ? (double)x.Current / x.Max : 0)
            .Take(count)
            .ToListAsync();

        // Ahora x.Max ya es int, por lo que la tupla coincide
        return data.Select(x => (x.Info, x.Max, x.Current)).ToList();
    }
    
    public async Task<(int Active, int Expired, Dictionary<string, int> ByType)> 
        GetPlanStatsRawAsync(DateTime from, DateTime to)
    {
        var f = DateOnly.FromDateTime(from);
        var t = DateOnly.FromDateTime(to);

        var query = context.UserPlans
            .AsNoTracking()
            .Where(up => up.StartDate >= f && up.StartDate <= t);

        var active = await query.CountAsync(up => up.IsActive == true);
        var expired = await query.CountAsync(up => up.IsActive == false);

        var byType = await query
            .GroupBy(up => up.Plan.Name)
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Name, x => x.Count);

        return (active, expired, byType);
    }
    
    public async Task<(int Total, int New, int Active)> 
        GetUserStatsRawAsync(DateTime from, DateTime to)
    {
        var total = await context.Users.CountAsync();

        var newUsers = await context.Users
            .CountAsync(u => u.CreatedAt >= from && u.CreatedAt <= to);

        var active = await context.Bookings
            .AsNoTracking()
            .Where(b => b.CreatedAt >= from && b.CreatedAt <= to)
            .Select(b => b.UserId)
            .Distinct()
            .CountAsync();

        return (total, newUsers, active);
    }
}
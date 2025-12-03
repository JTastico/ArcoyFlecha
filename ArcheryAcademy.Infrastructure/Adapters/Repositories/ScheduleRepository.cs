using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArcheryAcademy.Infrastructure.Adapters.Repositories;

public class ScheduleRepository(ArcheryAcademyDbContext context) : GenericRepository<Schedule>(context), IScheduleRepository
{
    // 1. VALIDAR SOLAPAMIENTO (Lógica de Intervalos)
    public async Task<bool> HasOverlapAsync(Guid instructorId, DateTime startTime, DateTime endTime)
    {
        // La lógica de solapamiento universal es:
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(s => 
                s.InstructorId == instructorId &&
                s.IsActive == true &&
                startTime < s.EndTime && 
                endTime > s.StartTime);
    }

    // 2. CUPOS DISPONIBLES (Eager Loading + Count)
    public async Task<IEnumerable<Schedule>> GetSchedulesWithOccupancyAsync(DateTime from, DateTime to, Guid? instructorId = null)
    {
        var query = _dbSet
            .AsNoTracking()
            .Include(s => s.Instructor) // Traemos nombre del instructor
            // Incluimos las reservas para contar (EF Core optimizará esto si proyectamos)
            .Include(s => s.Bookings) 
            .Where(s => s.StartTime >= from && s.EndTime <= to && s.IsActive == true);

        if (instructorId.HasValue)
        {
            query = query.Where(s => s.InstructorId == instructorId);
        }

        return await query
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }
}
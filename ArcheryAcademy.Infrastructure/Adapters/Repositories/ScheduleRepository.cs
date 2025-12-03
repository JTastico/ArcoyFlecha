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

    
}
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArcheryAcademy.Infrastructure.Adapters.Repositories;

public class BookingRepository(ArcheryAcademyDbContext context) : GenericRepository<Booking>(context), IBookingRepository
{
    public async Task<int> CountActiveBookingsAsync(Guid scheduleId)
    {
        // Contamos solo las reservas confirmadas o pendientes (ignoramos canceladas)
        // Status: 1=Pending, 2=Confirmed, 3=Cancelled, 4=Completed
        return await _dbSet
            .AsNoTracking()
            .CountAsync(b => b.ScheduleId == scheduleId && (b.StatusId == 1 || b.StatusId == 2));
    }

    public async Task<bool> IsUserAlreadyBookedAsync(Guid userId, Guid scheduleId)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(b => b.UserId == userId && 
                           b.ScheduleId == scheduleId && 
                           (b.StatusId == 1 || b.StatusId == 2));
    }

    public async Task<IEnumerable<Booking>> GetHistoryByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(b => b.Schedule)             // Datos del horario
            .ThenInclude(s => s.Instructor)   // Datos del instructor
            .Include(b => b.Status)        // Nombre del estado
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.Schedule.StartTime)
            .ToListAsync();
    }
}
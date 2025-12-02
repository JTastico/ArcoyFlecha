using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArcheryAcademy.Infrastructure.Adapters.Repositories;

public class UserPlanRepository(ArcheryAcademyDbContext context) : GenericRepository<UserPlan>(context), IUserPlanRepository
{
    // Implemetnacion de Validar si ya tiene plan activo
    public async Task<bool> HasActivePlanAsync(Guid userId)
    {
        return await _dbSet
            .AnyAsync(up => up.UserId == userId && up.IsActive == true);
    }
    // OBTENER ACTUAL: Usamos Eager Loading (Include) selectivo
    public async Task<UserPlan?> GetCurrentActivePlanAsync(Guid userId)
    {
        return await _dbSet
            .Include(up => up.Plan)
            // CORRECCIÓN: up.IsActive == true
            .Where(up => up.UserId == userId && up.IsActive == true)
            .FirstOrDefaultAsync();
    }

    // HISTORIAL: Proyección y Ordenamiento en BD
    public async Task<IEnumerable<UserPlan>> GetHistoryByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .AsNoTracking() // ⚡ IMPORTANTE: Lectura rápida (sin tracking de cambios)
            .Include(up => up.Plan) // Join con tabla Plans
            .Where(up => up.UserId == userId)
            .OrderByDescending(up => up.StartDate) // Ordenamos en BD
            .ToListAsync();
    }

    //  DESACTIVACIÓN AUTOMÁTICA
    public async Task<IEnumerable<UserPlan>> GetExpiredActivePlansAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return await _dbSet
            .Where(up => up.IsActive == true && up.EndDate < today)
            .ToListAsync();
    }
}
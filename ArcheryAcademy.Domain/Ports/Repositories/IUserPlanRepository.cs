using ArcheryAcademy.Domain.Entities;

namespace ArcheryAcademy.Domain.Ports;

public interface IUserPlanRepository : IGenericRepository<UserPlan>
{
    // Validar si ya tiene plan activo
    Task<bool> HasActivePlanAsync(Guid userId);
    // Obtener el plan activo actual con los datos del Plan (JOIN)
    Task<UserPlan?> GetCurrentActivePlanAsync(Guid userId);

    //  Historial de planes (Optimizado con AsNoTracking y OrderBy)
    Task<IEnumerable<UserPlan>> GetHistoryByUserIdAsync(Guid userId);

    //  Para desactivación automática (Batch processing)
    // Trae solo los IDs y datos necesarios de planes vencidos que siguen activos
    Task<IEnumerable<UserPlan>> GetExpiredActivePlansAsync();
}
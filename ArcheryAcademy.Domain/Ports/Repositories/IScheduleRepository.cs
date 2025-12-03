using ArcheryAcademy.Domain.Entities;

namespace ArcheryAcademy.Domain.Ports;

public interface IScheduleRepository : IGenericRepository<Schedule>
{
    // Validar Solapamientos:
    // Devuelve true si el instructor ya tiene clase en ese rango de horas
    Task<bool> HasOverlapAsync(Guid instructorId, DateTime startTime, DateTime endTime);
    
    // Disponibilidad y Cupos:
    // Trae los horarios de un rango de fechas con el conteo de inscritos ya calculado
    Task<IEnumerable<Schedule>> GetSchedulesWithOccupancyAsync(DateTime from, DateTime to, Guid? instructorId = null);

}
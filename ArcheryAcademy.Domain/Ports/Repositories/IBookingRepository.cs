using ArcheryAcademy.Domain.Entities;

namespace ArcheryAcademy.Domain.Ports;

public interface IBookingRepository : IGenericRepository<Booking>
{
    // Validar disponibilidad (Conteo rápido)
    Task<int> CountActiveBookingsAsync(Guid scheduleId);

    //  Evitar doble reserva
    Task<bool> IsUserAlreadyBookedAsync(Guid userId, Guid scheduleId);

    //  Historial con detalles (Include Schedule & Instructor)
    Task<IEnumerable<Booking>> GetHistoryByUserIdAsync(Guid userId);
}
namespace ArcheryAcademy.Application.DTOs.BookingDto;

public class BookingCreateDto
{
    public Guid UserId { get; set; }
    public Guid ScheduleId { get; set; }
    
    // CAMPOS FALTANTES QUE SON OBLIGATORIOS EN LA BD
    public Guid UserPlanId { get; set; }
    public int StatusId { get; set; } // ID del estado de la reserva (ej: 1=Pendiente)
    public int PaymentStatusId { get; set; } // ID del estado de pago (ej: 1=Pendiente)
}
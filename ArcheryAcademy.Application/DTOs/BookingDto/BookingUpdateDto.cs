namespace ArcheryAcademy.Application.DTOs.BookingDto;

public class BookingUpdateDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ScheduleId { get; set; }
    public Guid UserPlanId { get; set; }
    public int StatusId { get; set; }
    public int PaymentStatusId { get; set; }
    public DateTime? AttendedAt { get; set; }
}
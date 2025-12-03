namespace ArcheryAcademy.Application.DTOs.BookingDto;

public class BookingReadDto
{
    public Guid Id { get; set; }
    public string StudentName { get; set; } = string.Empty;  
    public string ScheduleInfo { get; set; } = string.Empty; 
    public string Status { get; set; } = string.Empty;       
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime? AttendedAt { get; set; }
}
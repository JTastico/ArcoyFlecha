namespace ArcheryAcademy.Application.DTOs.BookingDto;

public class BookingCreateDto
{
    public Guid UserId { get; set; }   
    public Guid ScheduleId { get; set; } 
}
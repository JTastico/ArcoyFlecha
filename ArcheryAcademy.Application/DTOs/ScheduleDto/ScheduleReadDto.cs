namespace ArcheryAcademy.Application.DTOs.ScheduleDto;

public class ScheduleReadDto
{
    public Guid Id { get; set; }
    public string InstructorName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int MaxStudents { get; set; }
    public int CurrentBookings { get; set; }
    public int AvailableSpots { get; set; }
}
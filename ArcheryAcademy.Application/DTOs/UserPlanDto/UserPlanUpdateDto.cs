namespace ArcheryAcademy.Application.DTOs.UserPlanDto;

public class UserPlanUpdateDto
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public int? RemainingClasses { get; set; } 
}
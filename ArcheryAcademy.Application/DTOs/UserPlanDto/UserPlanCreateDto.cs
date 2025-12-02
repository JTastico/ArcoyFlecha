namespace ArcheryAcademy.Application.DTOs.UserPlanDto;

public class UserPlanCreateDto
{
    public Guid UserId { get; set; }
    public Guid PlanId { get; set; }
    public DateOnly? StartDate { get; set; }
}
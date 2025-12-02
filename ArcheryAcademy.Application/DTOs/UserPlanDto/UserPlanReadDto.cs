namespace ArcheryAcademy.Application.DTOs.UserPlanDto;

public class UserPlanReadDto
{
    public Guid Id { get; set; } // Agregamos el ID del UserPlan generado
    public Guid UserId { get; set; }
    public Guid PlanId { get; set; }
    public string PlanName { get; set; } = string.Empty; // Nuevo: Nombre del plan
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int? RemainingClasses { get; set; }
    public bool IsActive { get; set; }
}
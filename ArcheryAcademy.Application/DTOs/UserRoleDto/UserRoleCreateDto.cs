namespace ArcheryAcademy.Application.DTOs.UserRoleDto;

public class UserRoleCreateDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime? AssignedAt { get; set; }
}
namespace ArcheryAcademy.Application.DTOs.UserRoleDto;

public class UserRoleUpdateDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime? AssignedAt { get; set; }
}
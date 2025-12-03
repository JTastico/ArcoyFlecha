namespace ArcheryAcademy.Application.DTOs.UserDto;

public class UserUpdateDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public Guid? RoleId { get; set; }
}
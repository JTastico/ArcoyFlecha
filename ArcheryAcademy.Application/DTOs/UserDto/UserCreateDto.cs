namespace ArcheryAcademy.Application.DTOs.UserDto;

public class UserCreateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // Contraseña plana
    public string? Phone { get; set; }
    public Guid RoleId { get; set; } // El rol que se le asignará
}
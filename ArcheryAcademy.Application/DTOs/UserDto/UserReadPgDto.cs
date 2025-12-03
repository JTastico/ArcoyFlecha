namespace ArcheryAcademy.Application.DTOs.UserDto;

public class UserReadPgDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new(); // Devolvemos los nombres de los roles
    public DateTime CreatedAt { get; set; }
}
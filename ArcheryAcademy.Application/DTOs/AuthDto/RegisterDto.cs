namespace ArcheryAcademy.Application.DTOs.AuthDto;

public class RegisterDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    // Nuevo campo: "A" o "I" (Default: "A")
    public string Status { get; set; } = "A";
    public string Role { get; set; } = "Alumno";
}
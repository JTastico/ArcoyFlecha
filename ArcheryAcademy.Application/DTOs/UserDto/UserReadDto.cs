using DocumentFormat.OpenXml.Wordprocessing;

namespace ArcheryAcademy.Application.DTOs.UserDto;

public class UserReadDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
}
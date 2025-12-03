namespace ArcheryAcademy.Application.DTOs.UserDto;

public class UserStatusUpdateDto
{
    // Filtro de busqueda
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    // El valor a actualizar
    public string Status { get; set; } = "A";
}
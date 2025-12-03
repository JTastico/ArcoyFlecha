namespace ArcheryAcademy.Application.DTOs.RolDto;

public class RolReadDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
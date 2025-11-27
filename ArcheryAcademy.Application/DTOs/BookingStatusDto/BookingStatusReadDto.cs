namespace ArcheryAcademy.Application.DTOs.BookingStatusDto;

public class BookingStatusReadDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public int? DisplayOrder { get; set; }
    public DateTime? CreatedAt { get; set; }
}
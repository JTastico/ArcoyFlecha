namespace ArcheryAcademy.Application.DTOs.PaymentStatusDto;

public class PaymentStatusReadDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public int? DisplayOrder { get; set; }
    public DateTime? CreatedAt { get; set; }
}
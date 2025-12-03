namespace ArcheryAcademy.Application.DTOs.PaymentStatusDto;

public class PaymentStatusCreateDto
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public int? DisplayOrder { get; set; }
}
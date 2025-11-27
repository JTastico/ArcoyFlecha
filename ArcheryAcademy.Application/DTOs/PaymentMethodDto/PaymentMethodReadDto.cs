namespace ArcheryAcademy.Application.DTOs.PaymentMethodDto;

public class PaymentMethodReadDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public int? DisplayOrder { get; set; }
    public DateTime? CreatedAt { get; set; }
}
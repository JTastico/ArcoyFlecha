using System.ComponentModel.DataAnnotations;

namespace ArcheryAcademy.Application.DTOs.PaymentMethodDto;

public class PaymentMethodUpdateDto
{
    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public int? DisplayOrder { get; set; }
}
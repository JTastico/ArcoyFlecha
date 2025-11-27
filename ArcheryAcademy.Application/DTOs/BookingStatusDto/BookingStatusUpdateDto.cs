using System.ComponentModel.DataAnnotations;

namespace ArcheryAcademy.Application.DTOs.BookingStatusDto;

public class BookingStatusUpdateDto
{
    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public int? DisplayOrder { get; set; }
}
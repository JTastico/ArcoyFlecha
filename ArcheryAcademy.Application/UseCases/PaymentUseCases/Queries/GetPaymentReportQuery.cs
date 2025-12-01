using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using ClosedXML.Excel;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.PaymentUseCases.Queries;

public record FileResultDto(string FileName, string ContentType, byte[] FileContent);

public record GetPaymentReportQuery(DateTime? StartDate, DateTime? EndDate) : IRequest<FileResultDto>;

internal sealed class GetPaymentReportQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetPaymentReportQuery, FileResultDto>
{
    public async Task<FileResultDto> Handle(GetPaymentReportQuery request, CancellationToken cancellationToken)
    {
        var payments = await unitOfWork.Repository<Payment>().FindWithIncludesAsync(
            x => (!request.StartDate.HasValue || x.CreatedAt >= request.StartDate) &&
                 (!request.EndDate.HasValue || x.CreatedAt <= request.EndDate),
            "Booking", "Booking.User", "Method", "Status"
        );

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Reporte de Pagos");
        
        worksheet.Cell(1, 1).Value = "Fecha";
        worksheet.Cell(1, 2).Value = "Alumno";
        worksheet.Cell(1, 3).Value = "Concepto";
        worksheet.Cell(1, 4).Value = "Método";
        worksheet.Cell(1, 5).Value = "Estado";
        worksheet.Cell(1, 6).Value = "Monto";

        var header = worksheet.Range("A1:F1");
        header.Style.Font.Bold = true;
        header.Style.Fill.BackgroundColor = XLColor.LightGray;

        int row = 2;
        foreach (var p in payments)
        {
            worksheet.Cell(row, 1).Value = p.CreatedAt?.ToString("yyyy-MM-dd HH:mm");
            worksheet.Cell(row, 2).Value = p.Booking?.User != null 
                ? $"{p.Booking.User.FirstName} {p.Booking.User.LastName}" 
                : "Anónimo";
            worksheet.Cell(row, 3).Value = $"Reserva {p.Booking?.Id.ToString().Substring(0, 4) ?? "?"}";
            worksheet.Cell(row, 4).Value = p.Method?.Name;
            worksheet.Cell(row, 5).Value = p.Status?.Name;
            worksheet.Cell(row, 6).Value = p.Amount;
            worksheet.Cell(row, 6).Style.NumberFormat.Format = "$ #,##0.00";
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return new FileResultDto(
            $"Reporte_Pagos_{DateTime.Now:yyyyMMdd}.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            stream.ToArray()
        );
    }
}
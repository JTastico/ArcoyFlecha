using ArcheryAcademy.Application.UseCases.PaymentUseCases.Queries;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using ClosedXML.Excel;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.ScheduleUseCase.Queries;

public record GetScheduleReportQuery(Guid ScheduleId) : IRequest<FileResultDto?>;

internal sealed class GetScheduleReportQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetScheduleReportQuery, FileResultDto?>
{
    public async Task<FileResultDto?> Handle(GetScheduleReportQuery request, CancellationToken cancellationToken)
    {
        var schedules = await unitOfWork.Repository<Schedule>().FindWithIncludesAsync(
            x => x.Id == request.ScheduleId,
            "Instructor", "Bookings", "Bookings.User", "Bookings.Status", "Bookings.PaymentStatus"
        );

        var schedule = schedules.FirstOrDefault();
        if (schedule == null) return null;

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Asistencia");

        var titleRange = worksheet.Range("A1:F1").Merge();
        titleRange.Value = "🏹 ARCHERY ACADEMY";
        titleRange.Style.Font.FontSize = 18;
        titleRange.Style.Font.Bold = true;
        titleRange.Style.Font.FontColor = XLColor.White;
        titleRange.Style.Fill.BackgroundColor = XLColor.DarkBlue;
        titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


        var subtitleRange = worksheet.Range("A2:F2").Merge();
        subtitleRange.Value = "LISTA DE CONTROL DE ASISTENCIA";
        subtitleRange.Style.Font.FontSize = 12;
        subtitleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        subtitleRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;


        var infoRange = worksheet.Range("B4:E6");
        infoRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
        
        // Fecha
        worksheet.Cell(4, 2).Value = "📅 Fecha:";
        worksheet.Cell(4, 2).Style.Font.Bold = true;
        worksheet.Cell(4, 3).Value = schedule.StartTime.ToString("dddd, dd MMMM yyyy");
        
        // Horario
        worksheet.Cell(5, 2).Value = "⏰ Horario:";
        worksheet.Cell(5, 2).Style.Font.Bold = true;
        worksheet.Cell(5, 3).Value = $"{schedule.StartTime:HH:mm} - {schedule.EndTime:HH:mm}";

        // Instructor
        worksheet.Cell(4, 4).Value = "👨‍🏫 Instructor:";
        worksheet.Cell(4, 4).Style.Font.Bold = true;
        worksheet.Cell(4, 5).Value = schedule.Instructor != null 
            ? $"{schedule.Instructor.FirstName} {schedule.Instructor.LastName}" 
            : "Por asignar";

        // Estadísticas
        int totalInscritos = schedule.Bookings.Count;
        int capacidad = schedule.MaxStudents ?? 0;
        
        worksheet.Cell(5, 4).Value = "📊 Ocupación:";
        worksheet.Cell(5, 4).Style.Font.Bold = true;
        worksheet.Cell(5, 5).Value = $"{totalInscritos} / {capacidad} Alumnos";
        
        if (totalInscritos >= capacidad)
        {
            worksheet.Cell(5, 5).Style.Font.FontColor = XLColor.Red;
            worksheet.Cell(5, 5).Value += " (LLENO)";
        }

        int row = 8;
        
        worksheet.Cell(row, 1).Value = "#";
        worksheet.Cell(row, 2).Value = "ALUMNO";
        worksheet.Cell(row, 3).Value = "CONTACTO (EMAIL)";
        worksheet.Cell(row, 4).Value = "ESTADO RESERVA";
        worksheet.Cell(row, 5).Value = "ESTADO PAGO";
        worksheet.Cell(row, 6).Value = "FIRMA";

        var headerTable = worksheet.Range(row, 1, row, 6);
        headerTable.Style.Font.Bold = true;
        headerTable.Style.Font.FontColor = XLColor.White;
        headerTable.Style.Fill.BackgroundColor = XLColor.SlateGray;
        headerTable.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        row++;
        int count = 1;

        foreach (var booking in schedule.Bookings)
        {
            worksheet.Cell(row, 1).Value = count++;
            worksheet.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Cell(row, 2).Value = booking.User != null 
                ? $"{booking.User.LastName}, {booking.User.FirstName}"
                : "Desconocido";

            worksheet.Cell(row, 3).Value = booking.User?.Email ?? "-";
            worksheet.Cell(row, 4).Value = booking.Status?.Name ?? "-";
            
            string pagoStatus = booking.PaymentStatus?.Name ?? "-";
            var cellPago = worksheet.Cell(row, 5);
            cellPago.Value = pagoStatus;
            cellPago.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Formato Condicional
            if (pagoStatus.ToUpper() != "PAGADO" && pagoStatus.ToUpper() != "PAID")
            {
                cellPago.Style.Font.FontColor = XLColor.Red;
                cellPago.Style.Font.Bold = true;
                cellPago.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFEBEE");
            }
            else
            {
                cellPago.Style.Font.FontColor = XLColor.Green;
            }

            worksheet.Cell(row, 6).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            
            // Zebra Striping
            if (count % 2 != 0)
            {
                worksheet.Range(row, 1, row, 6).Style.Fill.BackgroundColor = XLColor.FromHtml("#F5F5F5");
            }

            row++;
        }

        worksheet.Columns().AdjustToContents();
        worksheet.Column(6).Width = 20;
        
        worksheet.SheetView.FreezeRows(8);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return new FileResultDto(
            $"Lista_{schedule.StartTime:yyyyMMdd}_{schedule.Id.ToString().Substring(0,4)}.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            stream.ToArray()
        );
    }
}
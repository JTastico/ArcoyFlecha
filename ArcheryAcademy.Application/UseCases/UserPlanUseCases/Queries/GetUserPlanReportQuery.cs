using ArcheryAcademy.Application.UseCases.PaymentUseCases.Queries;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using ClosedXML.Excel;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserPlanUseCases.Queries;

// 1. Query
public record GetUserPlanReportQuery(int DaysThreshold = 7) : IRequest<FileResultDto>;

// 2. Handler
internal sealed class GetUserPlanReportQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUserPlanReportQuery, FileResultDto>
{
    public async Task<FileResultDto> Handle(GetUserPlanReportQuery request, CancellationToken cancellationToken)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        DateOnly limitDate = today.AddDays(request.DaysThreshold);

        var userPlans = await unitOfWork.Repository<UserPlan>().FindWithIncludesAsync(
            x => x.IsActive == true && x.EndDate <= limitDate,
            "User", "Plan"
        );
        
        userPlans = userPlans.OrderBy(x => x.EndDate).ToList();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Vencimientos");

        // --- ENCABEZADO PRO (CORREGIDO) ---
        worksheet.Cell(1, 1).Value = "🏹 ARCHERY ACADEMY - RETENCIÓN";
        var titleRange = worksheet.Range("A1:G1").Merge();
        titleRange.Style.Font.FontSize = 16;
        titleRange.Style.Font.Bold = true;
        titleRange.Style.Font.FontColor = XLColor.White;
        titleRange.Style.Fill.BackgroundColor = XLColor.DarkRed;
        titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        worksheet.Cell(2, 1).Value = $"REPORTE DE VENCIMIENTOS (Próximos {request.DaysThreshold} días)";
        var subtitleRange = worksheet.Range("A2:G2").Merge();
        subtitleRange.Style.Font.FontSize = 12;
        subtitleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        subtitleRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

        // --- TABLA DE DATOS ---
        int row = 4;
        worksheet.Cell(row, 1).Value = "ALUMNO";
        worksheet.Cell(row, 2).Value = "PLAN ACTUAL";
        worksheet.Cell(row, 3).Value = "CONTACTO";
        worksheet.Cell(row, 4).Value = "CLASES RESTANTES";
        worksheet.Cell(row, 5).Value = "FECHA FIN";
        worksheet.Cell(row, 6).Value = "DÍAS RESTANTES";
        worksheet.Cell(row, 7).Value = "ESTADO";

        // Estilos de cabecera de tabla (CORREGIDO)
        var header = worksheet.Range(row, 1, row, 7);
        header.Style.Font.Bold = true;
        header.Style.Font.FontColor = XLColor.White;
        header.Style.Fill.BackgroundColor = XLColor.DimGray;
        header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        row++;

        foreach (var up in userPlans)
        {
            // Datos básicos
            worksheet.Cell(row, 1).Value = up.User != null ? $"{up.User.LastName}, {up.User.FirstName}" : "-";
            worksheet.Cell(row, 2).Value = up.Plan?.Name ?? "-";
            worksheet.Cell(row, 3).Value = up.User?.Email ?? up.User?.Phone ?? "No data";
            worksheet.Cell(row, 4).Value = up.RemainingClasses;
            worksheet.Cell(row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Fechas
            worksheet.Cell(row, 5).Value = up.EndDate.ToString("dd/MM/yyyy");
            
            // Cálculo de días
            int daysLeft = up.EndDate.DayNumber - today.DayNumber;
            worksheet.Cell(row, 6).Value = daysLeft;
            worksheet.Cell(row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Lógica de Estado (Semáforo)
            var cellStatus = worksheet.Cell(row, 7);
            
            if (daysLeft < 0)
            {
                cellStatus.Value = "VENCIDO";
                cellStatus.Style.Fill.BackgroundColor = XLColor.Red;
                cellStatus.Style.Font.FontColor = XLColor.White;
            }
            else if (daysLeft == 0)
            {
                cellStatus.Value = "VENCE HOY";
                cellStatus.Style.Fill.BackgroundColor = XLColor.OrangeRed;
                cellStatus.Style.Font.FontColor = XLColor.White;
            }
            else if (daysLeft <= 3)
            {
                cellStatus.Value = "CRÍTICO";
                cellStatus.Style.Fill.BackgroundColor = XLColor.Orange;
            }
            else
            {
                cellStatus.Value = "POR VENCER";
                cellStatus.Style.Fill.BackgroundColor = XLColor.Yellow;
            }
            
            cellStatus.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cellStatus.Style.Font.Bold = true;

            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return new FileResultDto(
            $"Vencimientos_{DateTime.Now:yyyyMMdd}.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            stream.ToArray()
        );
    }
}
namespace ArcheryAcademy.Domain.Ports;

public interface IReportRepository
{
    // Retorna: Total, TotalSemana, Diccionario<EstadoId, Cantidad>
    Task<(int TotalToday, int TotalRange, Dictionary<int, int> StatusCounts)> GetBookingStatsRawAsync(DateTime from, DateTime to);

}
namespace ArcheryAcademy.Domain.Ports;

public interface IReportRepository
{
    // Retorna: Total, TotalSemana, Diccionario<EstadoId, Cantidad>
    Task<(int TotalToday, int TotalRange, Dictionary<int, int> StatusCounts)> GetBookingStatsRawAsync(DateTime from, DateTime to);
    // Retorna lista de objetos anónimos (o tuplas) con la info de la clase
    Task<List<(string Info, int Max, int Current)>> GetTopClassesRawAsync(DateTime from, DateTime to, int count = 5);
}
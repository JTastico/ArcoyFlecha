using ArcheryAcademy.Application.DTOs.ReportDto;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.ReportsUsesCases.Queries;

public record GetBookingStatsQuery(DateTime From, DateTime To) : IRequest<BookingStatsDto>;

// 2. El Handler
internal sealed class GetBookingStatsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetBookingStatsQuery, BookingStatsDto>
{
    public async Task<BookingStatsDto> Handle(GetBookingStatsQuery request, CancellationToken cancellationToken)
    {
        // Validación básica de fechas
        if (request.From > request.To)
            throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.");

        // A. Llamamos al repositorio que devuelve la Tupla: (TotalToday, TotalRange, Diccionario)
        var rawStats = await unitOfWork.ReportRepository.GetBookingStatsRawAsync(request.From, request.To);

        // B. El Mapper transforma la Tupla -> BookingStatsDto (Usando tu ReportMappingProfile)
        return mapper.Map<BookingStatsDto>(rawStats);
    }
}
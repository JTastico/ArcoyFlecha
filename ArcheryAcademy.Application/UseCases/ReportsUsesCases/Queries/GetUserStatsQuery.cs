using ArcheryAcademy.Application.DTOs.ReportDto;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.ReportsUsesCases.Queries;

public record GetUserStatsQuery(DateTime From, DateTime To) : IRequest<UserStatsDto>;

// 2. Handler
internal sealed class GetUserStatsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetUserStatsQuery, UserStatsDto>
{
    public async Task<UserStatsDto> Handle(GetUserStatsQuery request, CancellationToken cancellationToken)
    {
        // A. Obtener datos crudos del repositorio
        var rawStats = await unitOfWork.ReportRepository.GetUserStatsRawAsync(request.From, request.To);

        // B. Mapear a DTO
        return mapper.Map<UserStatsDto>(rawStats);
    }
}
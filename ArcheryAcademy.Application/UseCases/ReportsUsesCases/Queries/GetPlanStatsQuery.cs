using ArcheryAcademy.Application.DTOs.ReportDto;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.ReportsUsesCases.Queries;

public record GetPlanStatsQuery(DateTime From, DateTime To) : IRequest<PlanStatsDto>;

// 2. Handler
internal sealed class GetPlanStatsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetPlanStatsQuery, PlanStatsDto>
{
    public async Task<PlanStatsDto> Handle(GetPlanStatsQuery request, CancellationToken cancellationToken)
    {
        // A. Obtener datos crudos (Tupla) del repositorio
        // Nota: Asumo que el método en el repo se llama GetPlanStatsRawAsync
        var rawStats = await unitOfWork.ReportRepository.GetPlanStatsRawAsync(request.From, request.To);

        // B. Mapear a DTO
        return mapper.Map<PlanStatsDto>(rawStats);
    }
}
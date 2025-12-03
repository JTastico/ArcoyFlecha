using ArcheryAcademy.Application.DTOs.ReportDto;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.ReportsUsesCases.Queries;

public record GetTopClassesQuery(DateTime From, DateTime To) : IRequest<List<ClassOccupancyDto>>;

// 2. Handler
internal sealed class GetTopClassesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetTopClassesQuery, List<ClassOccupancyDto>>
{
    public async Task<List<ClassOccupancyDto>> Handle(GetTopClassesQuery request, CancellationToken cancellationToken)
    {
        // A. Llamamos al repositorio (Método que ya tienes implementado con LINQ)
        // Nota: Asumimos que el método en el repo se llama GetTopClassesRawAsync
        var rawList = await unitOfWork.ReportRepository.GetTopClassesRawAsync(request.From, request.To);

        // B. Mapeamos la lista de Tuplas a lista de DTOs
        return mapper.Map<List<ClassOccupancyDto>>(rawList);
    }
}
using ArcheryAcademy.Application.DTOs.UserPlanDto;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserPlanUseCases.Queries;

public record GetUserPlansHistoryQuery(Guid UserId) : IRequest<List<UserPlanReadDto>>;

internal sealed class GetUserPlansHistoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetUserPlansHistoryQuery, List<UserPlanReadDto>>
{
    public async Task<List<UserPlanReadDto>> Handle(GetUserPlansHistoryQuery request, CancellationToken cancellationToken)
    {
        var history = await unitOfWork.UserPlanRepository.GetHistoryByUserIdAsync(request.UserId);
        return mapper.Map<List<UserPlanReadDto>>(history);
    }
}
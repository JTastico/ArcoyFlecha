using ArcheryAcademy.Application.DTOs.PaymentStatusDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.PaymentStatusUseCases.Queries;

public record GetAllPaymentStatusesQuery() : IRequest<List<PaymentStatusReadDto>>;

internal sealed class GetAllPaymentStatusesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllPaymentStatusesQuery, List<PaymentStatusReadDto>>
{
    public async Task<List<PaymentStatusReadDto>> Handle(GetAllPaymentStatusesQuery request, CancellationToken cancellationToken)
    {
        var list = await unitOfWork.Repository<PaymentStatus>().GetAllAsync();
        return mapper.Map<List<PaymentStatusReadDto>>(list);
    }
}

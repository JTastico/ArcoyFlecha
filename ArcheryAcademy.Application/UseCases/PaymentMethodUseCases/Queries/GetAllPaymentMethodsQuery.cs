using ArcheryAcademy.Application.DTOs.PaymentMethodDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.PaymentMethodUseCases.Queries;

public record GetAllPaymentMethodsQuery() : IRequest<List<PaymentMethodReadDto>>;

internal sealed class GetAllPaymentMethodsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllPaymentMethodsQuery, List<PaymentMethodReadDto>>
{
    public async Task<List<PaymentMethodReadDto>> Handle(GetAllPaymentMethodsQuery request, CancellationToken cancellationToken)
    {
        var paymentMethods = await unitOfWork.Repository<PaymentMethod>().GetAllAsync();
        return mapper.Map<List<PaymentMethodReadDto>>(paymentMethods);
    }
}
using ArcheryAcademy.Application.DTOs.PaymentMethodDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.PaymentMethodUseCases.Queries;

public record GetPaymentMethodByIdQuery(int Id) : IRequest<PaymentMethodReadDto?>;

internal sealed class GetPaymentMethodByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetPaymentMethodByIdQuery, PaymentMethodReadDto?>
{
    public async Task<PaymentMethodReadDto?> Handle(GetPaymentMethodByIdQuery request, CancellationToken cancellationToken)
    {
        var paymentMethod = await unitOfWork.Repository<PaymentMethod>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        return paymentMethod == null ? null : mapper.Map<PaymentMethodReadDto>(paymentMethod);
    }
}
using ArcheryAcademy.Application.DTOs.PaymentMethodDto;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.PaymentMethodUseCases.Command;

public record CreatePaymentMethodCommand(PaymentMethodCreateDto PaymentMethodDto) : IRequest<PaymentMethod>;

internal sealed class CreatePaymentMethodCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreatePaymentMethodCommand, PaymentMethod>
{
    public async Task<PaymentMethod> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var paymentMethod = mapper.Map<PaymentMethod>(request.PaymentMethodDto);

        await unitOfWork.Repository<PaymentMethod>().Insert(paymentMethod);
        await unitOfWork.CompleteAsync(cancellationToken);
        return paymentMethod;
    }
}
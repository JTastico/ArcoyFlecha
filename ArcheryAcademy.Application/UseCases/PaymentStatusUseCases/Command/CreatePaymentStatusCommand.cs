using ArcheryAcademy.Application.DTOs.PaymentStatusDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.PaymentStatusUseCases.Command;

public record CreatePaymentStatusCommand(PaymentStatusCreateDto PaymentStatusDto) : IRequest<PaymentStatus>;

internal sealed class CreatePaymentStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreatePaymentStatusCommand, PaymentStatus>
{
    public async Task<PaymentStatus> Handle(CreatePaymentStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<PaymentStatus>(request.PaymentStatusDto);

        await unitOfWork.Repository<PaymentStatus>().Insert(entity);
        await unitOfWork.CompleteAsync(cancellationToken);

        return entity;
    }
}

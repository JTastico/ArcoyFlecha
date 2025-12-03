using ArcheryAcademy.Application.DTOs.PaymentStatusDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.PaymentStatusUseCases.Command;

public record UpdatePaymentStatusCommand(int Id, PaymentStatusUpdateDto PaymentStatusDto) 
    : IRequest<PaymentStatus?>;

internal sealed class UpdatePaymentStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdatePaymentStatusCommand, PaymentStatus?>
{
    public async Task<PaymentStatus?> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
    {
        var existing = await unitOfWork.Repository<PaymentStatus>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (existing == null)
            return null;

        mapper.Map(request.PaymentStatusDto, existing);

        await unitOfWork.Repository<PaymentStatus>().UpdateAsync(existing);
        await unitOfWork.CompleteAsync(cancellationToken);

        return existing;
    }
}
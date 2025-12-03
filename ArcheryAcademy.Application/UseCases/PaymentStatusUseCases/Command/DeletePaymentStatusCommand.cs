using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.PaymentStatusUseCases.Command;

public record DeletePaymentStatusCommand(int Id) : IRequest<bool>;

internal sealed class DeletePaymentStatusCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePaymentStatusCommand, bool>
{
    public async Task<bool> Handle(DeletePaymentStatusCommand request, CancellationToken cancellationToken)
    {
        var repo = unitOfWork.Repository<PaymentStatus>();
        var entity = await repo.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
            return false;

        await repo.DeleteAsync(entity, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return true;
    }
}
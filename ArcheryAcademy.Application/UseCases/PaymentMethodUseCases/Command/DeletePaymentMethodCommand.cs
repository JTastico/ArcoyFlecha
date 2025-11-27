using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.PaymentMethodUseCases.Command;

public record DeletePaymentMethodCommand(int Id) : IRequest<bool>;

internal sealed class DeletePaymentMethodCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePaymentMethodCommand, bool>
{
    public async Task<bool> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.Repository<PaymentMethod>();
        var paymentMethod = await repository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (paymentMethod == null)
            return false;

        await repository.DeleteAsync(paymentMethod, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return true;
    }
}
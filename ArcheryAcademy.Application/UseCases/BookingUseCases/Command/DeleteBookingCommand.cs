using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.BookingUseCases.Command;

public record DeleteBookingCommand(Guid Id) : IRequest<bool>;

internal sealed class DeleteBookingCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteBookingCommand, bool>
{
    public async Task<bool> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.Repository<Booking>();
        var booking = await repository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (booking == null)
            return false;

        await repository.DeleteAsync(booking, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return true;
    }
}

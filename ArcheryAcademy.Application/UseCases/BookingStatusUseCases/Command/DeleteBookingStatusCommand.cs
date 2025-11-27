using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.BookingStatusUseCases.Command;

public record DeleteBookingStatusCommand(int Id) : IRequest<bool>;

internal sealed class DeleteBookingStatusCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteBookingStatusCommand, bool>
{
    public async Task<bool> Handle(DeleteBookingStatusCommand request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.Repository<BookingStatus>();
        var bookingStatus = await repository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (bookingStatus == null)
            return false;

        await repository.DeleteAsync(bookingStatus, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return true;
    }
}
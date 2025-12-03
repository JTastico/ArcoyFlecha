using ArcheryAcademy.Domain.Ports;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.BookingUseCases.Command;

public record RegisterAttendanceCommand(Guid BookingId) : IRequest<bool>;

internal sealed class RegisterAttendanceCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterAttendanceCommand, bool>
{
    public async Task<bool> Handle(RegisterAttendanceCommand request, CancellationToken cancellationToken)
    {
        var booking = await unitOfWork.BookingRepository.GetByIdAsync(request.BookingId);
        if (booking == null) return false;

        // Marcar asistencia y completar
        booking.AttendedAt = DateTime.UtcNow;
        booking.StatusId = 4; // Completed

        await unitOfWork.BookingRepository.UpdateAsync(booking);
        await unitOfWork.CompleteAsync(cancellationToken);

        return true;
    }
}
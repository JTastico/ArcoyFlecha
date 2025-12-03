using ArcheryAcademy.Domain.Ports;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.BookingUseCases.Command;

public record CancelBookingCommand(Guid BookingId) : IRequest<bool>;

internal sealed class CancelBookingCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CancelBookingCommand, bool>
{
    public async Task<bool> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await unitOfWork.BookingRepository.GetByIdAsync(request.BookingId);
        if (booking == null) return false;

        // Si ya está cancelada (3) o completada (4), no hacemos nada
        if (booking.StatusId == 3 || booking.StatusId == 4) return true;

        // 1. Cambiar estado a Cancelado
        booking.StatusId = 3; // Cancelled

        // 2. REEMBOLSO (Devolver crédito al plan)
        var userPlan = await unitOfWork.UserPlanRepository.GetByIdAsync(booking.UserPlanId);
        if (userPlan != null)
        {
            userPlan.RemainingClasses += 1;
            await unitOfWork.UserPlanRepository.UpdateAsync(userPlan);
        }

        // 3. Guardar
        await unitOfWork.BookingRepository.UpdateAsync(booking);
        await unitOfWork.CompleteAsync(cancellationToken);

        return true;
    }
}
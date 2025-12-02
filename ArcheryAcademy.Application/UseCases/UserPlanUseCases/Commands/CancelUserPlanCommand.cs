using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserPlanUseCases.Commands;

public record CancelUserPlanCommand(Guid UserPlanId) : IRequest<bool>;

internal sealed class CancelUserPlanCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CancelUserPlanCommand, bool>
{
    public async Task<bool> Handle(CancelUserPlanCommand request, CancellationToken cancellationToken)
    {
        // Usamos el genérico porque buscar por ID es estándar
        var userPlan = await unitOfWork.Repository<UserPlan>().GetByIdAsync(request.UserPlanId);
        if (userPlan == null) return false;
        userPlan.IsActive = false;

        // Actualizamos
        await unitOfWork.Repository<UserPlan>().UpdateAsync(userPlan);
        await unitOfWork.CompleteAsync(cancellationToken);

        return true;
    }
}
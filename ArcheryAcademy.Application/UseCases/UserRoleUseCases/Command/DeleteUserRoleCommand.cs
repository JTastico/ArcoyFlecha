using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserRoleUseCases.Command;

public record DeleteUserRoleCommand(Guid Id) : IRequest<bool>;

internal sealed class DeleteUserRoleCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteUserRoleCommand, bool>
{
    public async Task<bool> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.Repository<UserRole>();
        var userRole = await repository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (userRole == null)
            return false;

        await repository.DeleteAsync(userRole, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return true;
    }
}

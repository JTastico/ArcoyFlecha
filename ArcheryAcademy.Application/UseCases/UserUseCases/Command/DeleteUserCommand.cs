using ArcheryAcademy.Application.Exceptions;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserUseCases.Command;

public record DeleteUserCommand(Guid Id) : IRequest; 

internal sealed class DeleteUserCommandHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Repository<User>().GetByIdAsync(request.Id);

        if (user == null)
        {
            // LANZAMOS la excepción. El Middleware la atrapará y devolverá 404.
            throw new NotFoundException("User", request.Id);
        }

        // Si falla por FK, Entity Framework lanza DbUpdateException automáticamente.
        // El Middleware la atrapará y devolverá 400.
        await unitOfWork.Repository<User>().DeleteAsync(user);
        await unitOfWork.CompleteAsync(cancellationToken);
    }
}
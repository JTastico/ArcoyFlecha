using ArcheryAcademy.Application.DTOs.UserDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserUseCases.Command;

public record UpdateUserStatusCommand(UserStatusUpdateDto StatusDto) : IRequest<bool>;

internal sealed class UpdateUserStatusCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateUserStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Repository<User>().FirstOrDefaultAsync(
            u => u.FirstName.ToLower() == request.StatusDto.FirstName.ToLower() &&
                 u.LastName.ToLower() == request.StatusDto.LastName.ToLower(),
            cancellationToken
        );

        if (user == null) return false;
        
        user.Status = request.StatusDto.Status;

        await unitOfWork.Repository<User>().UpdateAsync(user);
        await unitOfWork.CompleteAsync(cancellationToken);

        return true;
    }
}
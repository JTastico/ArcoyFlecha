using ArcheryAcademy.Application.DTOs.UserRoleDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserRoleUseCases.Command;

public record UpdateUserRoleCommand(Guid Id, UserRoleUpdateDto UserRoleDto) 
    : IRequest<UserRole?>;

internal sealed class UpdateUserRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateUserRoleCommand, UserRole?>
{
    public async Task<UserRole?> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var existing = await unitOfWork.Repository<UserRole>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (existing is null)
            return null;

        mapper.Map(request.UserRoleDto, existing);

        await unitOfWork.Repository<UserRole>().UpdateAsync(existing);
        await unitOfWork.CompleteAsync(cancellationToken);

        return existing;
    }
}

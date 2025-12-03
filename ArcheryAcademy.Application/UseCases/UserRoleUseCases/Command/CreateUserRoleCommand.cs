using ArcheryAcademy.Application.DTOs.UserRoleDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserRoleUseCases.Command;

public record CreateUserRoleCommand(UserRoleCreateDto UserRoleDto) : IRequest<UserRole>;

internal sealed class CreateUserRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateUserRoleCommand, UserRole>
{
    public async Task<UserRole> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var userRole = mapper.Map<UserRole>(request.UserRoleDto);

        await unitOfWork.Repository<UserRole>().Insert(userRole);
        await unitOfWork.CompleteAsync(cancellationToken);

        return userRole;
    }
}

using ArcheryAcademy.Application.DTOs.UserRoleDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserRoleUseCases.Queries;

public record GetUserRoleByIdQuery(Guid Id) : IRequest<UserRoleReadDto?>;

internal sealed class GetUserRoleByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetUserRoleByIdQuery, UserRoleReadDto?>
{
    public async Task<UserRoleReadDto?> Handle(GetUserRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var userRole = await unitOfWork.Repository<UserRole>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return userRole == null ? null : mapper.Map<UserRoleReadDto>(userRole);
    }
}

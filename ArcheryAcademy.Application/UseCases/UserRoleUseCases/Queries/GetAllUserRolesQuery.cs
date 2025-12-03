using ArcheryAcademy.Application.DTOs.UserRoleDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserRoleUseCases.Queries;

public record GetAllUserRolesQuery() : IRequest<List<UserRoleReadDto>>;

internal sealed class GetAllUserRolesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllUserRolesQuery, List<UserRoleReadDto>>
{
    public async Task<List<UserRoleReadDto>> Handle(GetAllUserRolesQuery request, CancellationToken cancellationToken)
    {
        var userRoles = await unitOfWork.Repository<UserRole>().GetAllAsync();
        return mapper.Map<List<UserRoleReadDto>>(userRoles);
    }
}

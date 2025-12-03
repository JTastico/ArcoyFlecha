using ArcheryAcademy.Application.DTOs.UserDto;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserUseCases.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<UserReadPgDto?>;

// 2. Handler
internal sealed class GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetUserByIdQuery, UserReadPgDto?>
{
    public async Task<UserReadPgDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {

        var user = await unitOfWork.UserRepository.GetByIdWithRolesAsync(request.Id);

        if (user == null) 
            return null;

        return mapper.Map<UserReadPgDto>(user);
    }
}
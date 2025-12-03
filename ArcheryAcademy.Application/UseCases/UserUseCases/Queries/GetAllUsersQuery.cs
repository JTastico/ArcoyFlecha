using ArcheryAcademy.Application.Common;
using ArcheryAcademy.Application.DTOs.UserDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserUseCases.Queries;

public class GetAllUsersQuery : IRequest<PagedResult<UserReadPgDto>>
{
    public int Page { get; set; } = 1;      // Valor por defecto
    public int PageSize { get; set; } = 10; // Valor por defecto
}

// 2. El Handler (Lógica)
internal sealed class GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllUsersQuery, PagedResult<UserReadPgDto>>
{
    public async Task<PagedResult<UserReadPgDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        // A. Usamos el Repositorio Genérico con la mejora de Paginación e Includes
        var (users, totalCount) = await unitOfWork.Repository<User>().GetPagedAsync(
            page: request.Page,
            pageSize: request.PageSize,
            filter: null, // Opcional: u => u.Status == "A" si quisieras solo activos
            orderBy: q => q.OrderByDescending(u => u.CreatedAt), // Ordenar por más recientes
            

            includeProperties: "UserRoles.Role" 
        );
        
        var dtos = mapper.Map<IEnumerable<UserReadPgDto>>(users);

        // C. Retornamos el objeto paginado estándar
        return new PagedResult<UserReadPgDto>(dtos, totalCount, request.Page, request.PageSize);
    }
}
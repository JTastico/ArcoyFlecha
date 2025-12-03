using ArcheryAcademy.Application.DTOs.UserDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserUseCases.Command;

public record UpdateUserCommand(Guid Id, UserUpdateDto UserDto) : IRequest<bool>;

internal sealed class UpdateUserCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateUserCommand, bool>
{
    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Buscar usuario INCLUYENDO sus roles
        var users = await unitOfWork.Repository<User>().FindWithIncludesAsync(
            u => u.Id == request.Id,
            "UserRoles" 
        );
        var user = users.FirstOrDefault();

        if (user == null) return false;

        // 2. Validar Email Único
        if (user.Email != request.UserDto.Email)
        {
            var emailTaken = await unitOfWork.Repository<User>()
                .FirstOrDefaultAsync(u => u.Email == request.UserDto.Email, cancellationToken);

            if (emailTaken != null)
            {
                // Aquí podrías lanzar una excepción personalizada o retornar false/Result
                throw new InvalidOperationException($"El email '{request.UserDto.Email}' ya está en uso.");
            }
            user.Email = request.UserDto.Email;
        }

        // 3. Actualizar datos básicos
        user.FirstName = request.UserDto.FirstName;
        user.LastName = request.UserDto.LastName;
        user.Phone = request.UserDto.Phone;

        // 4. Actualizar Rol
        if (request.UserDto.RoleId.HasValue && request.UserDto.RoleId != Guid.Empty)
        {
            // Validar que el nuevo rol exista
            var newRole = await unitOfWork.Repository<Role>()
                .GetByIdAsync(request.UserDto.RoleId.Value);

            if (newRole == null)
            {
                throw new KeyNotFoundException($"El rol con ID {request.UserDto.RoleId} no existe.");
            }

            // Limpiar roles anteriores
            user.UserRoles.Clear();

            // Agregar el nuevo rol
            user.UserRoles.Add(new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                RoleId = newRole.Id,
                AssignedAt = DateTime.UtcNow
            });
        }
        
        // 5. Guardar cambios
        await unitOfWork.Repository<User>().UpdateAsync(user);
        await unitOfWork.CompleteAsync(cancellationToken);

        return true;
    }
}
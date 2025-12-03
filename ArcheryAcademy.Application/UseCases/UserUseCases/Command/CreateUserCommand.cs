using ArcheryAcademy.Application.DTOs.UserDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Domain.Ports.Authentication;
using AutoMapper;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.UserUseCases.Command;

public record CreateUserCommand(UserCreateDto UserDto) : IRequest<UserReadPgDto>;

internal sealed class CreateUserCommandHandler(
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    IPasswordHasher passwordHasher
    ) 
    : IRequestHandler<CreateUserCommand, UserReadPgDto>
{
    public async Task<UserReadPgDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.UserDto;
        // 1. VALIDACIONES PREVIAS
        var existingUser = await unitOfWork.Repository<User>()
            .FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("El correo electrónico ya está registrado.");
        // B. Validar si el Rol existe
        var role = await unitOfWork.Repository<Role>().GetByIdAsync(dto.RoleId);
        if (role == null)
            throw new KeyNotFoundException($"El Rol con ID {dto.RoleId} no existe.");
        // 2. HASHEAR PASSWORD (Infraestructura)
        var passwordHash = passwordHasher.Hash(dto.Password);
        // 3. CREAR ENTIDAD USUARIO
        var newUser = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = passwordHash, // Guardamos el hash, no la plana
            Phone = dto.Phone,
            Status = "A", // Activo por defecto
            CreatedAt = DateTime.UtcNow
        };
        // 4. ASIGNAR ROL (Crear relación UserRole)
        var newUserRole = new UserRole
        {
            Id = Guid.NewGuid(),
            UserId = newUser.Id,
            RoleId = role.Id,
            AssignedAt = DateTime.UtcNow
        };
        // Opción A: Agregar a la colección de navegación (si está inicializada)
        newUser.UserRoles.Add(newUserRole);
        // 5. GUARDAR TODO (Atomicidad)
        await unitOfWork.Repository<User>().Insert(newUser);
        await unitOfWork.CompleteAsync(cancellationToken);
        // 6. RESPUESTA
        newUserRole.Role = role; 
        
        return mapper.Map<UserReadPgDto>(newUser);
    }
}
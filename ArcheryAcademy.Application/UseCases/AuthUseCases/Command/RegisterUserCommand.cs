using ArcheryAcademy.Application.DTOs.AuthDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Domain.Ports.Authentication;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.AuthUseCases.Command;

public record RegisterUserCommand(RegisterDto RegisterDto) : IRequest<AuthResponseDto?>;

internal sealed class RegisterUserCommandHandler(
    IUnitOfWork unitOfWork, 
    IJwtTokenGenerator tokenGenerator,
    IPasswordHasher passwordHasher) 
    : IRequestHandler<RegisterUserCommand, AuthResponseDto?>
{
    public async Task<AuthResponseDto?> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar Email
        var existingUser = await unitOfWork.Repository<User>()
            .FirstOrDefaultAsync(u => u.Email == request.RegisterDto.Email, cancellationToken);

        if (existingUser != null) return null;
        
        string roleName = string.IsNullOrWhiteSpace(request.RegisterDto.Role) 
            ? "Alumno" 
            : request.RegisterDto.Role;
        
        var selectedRole = await unitOfWork.Repository<Role>()
            .FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
        

        if (selectedRole == null) 
        {
            // Opcional: Podrías lanzar una excepción personalizada aquí
            throw new KeyNotFoundException($"El rol '{roleName}' no existe.");
        }
        
        
        // 2. Obtener Rol por defecto "Alumno"
        var defaultRole = await unitOfWork.Repository<Role>()
            .FirstOrDefaultAsync(r => r.Name == "Alumno", cancellationToken);

        if (defaultRole == null) 
            throw new Exception("El rol 'Alumno' no existe en la base de datos.");

        // 3. Crear Usuario
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.RegisterDto.FirstName,
            LastName = request.RegisterDto.LastName,
            Email = request.RegisterDto.Email,
            Phone = request.RegisterDto.Phone,
            Status = request.RegisterDto.Status,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = passwordHasher.Hash(request.RegisterDto.Password)
        };

        // 4. Asignar Rol
        newUser.UserRoles.Add(new UserRole
        {
            Id = Guid.NewGuid(),
            UserId = newUser.Id,
            RoleId = defaultRole.Id,
            AssignedAt = DateTime.UtcNow,
            Role = defaultRole
        });

        // 5. Guardar
        await unitOfWork.Repository<User>().Insert(newUser);
        await unitOfWork.CompleteAsync(cancellationToken);

        // 6. Generar Token
        var token = tokenGenerator.GenerateToken(newUser);

        return new AuthResponseDto(newUser.Id, newUser.Email, token);
    }
}
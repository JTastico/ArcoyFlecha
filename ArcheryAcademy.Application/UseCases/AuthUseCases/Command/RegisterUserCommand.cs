using ArcheryAcademy.Application.DTOs.AuthDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Domain.Ports.Authentication;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.AuthUseCases.Command;

// Comando: Recibe datos y devuelve la respuesta de Auth (con token incluido para autologin)
public record RegisterUserCommand(RegisterDto RegisterDto) : IRequest<AuthResponseDto?>;

internal sealed class RegisterUserCommandHandler(
    IUnitOfWork unitOfWork, 
    IJwtTokenGenerator tokenGenerator,
    IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterUserCommand, AuthResponseDto?>
{
    public async Task<AuthResponseDto?> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar si el email ya existe
        var existingUser = await unitOfWork.Repository<User>()
            .FirstOrDefaultAsync(u => u.Email == request.RegisterDto.Email, cancellationToken);

        if (existingUser != null) return null;

        
        // 2. Hasheamos la contraseña
        var passwordHash = passwordHasher.Hash(request.RegisterDto.Password);
        
        
        // 3. Crear la entidad Usuario
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.RegisterDto.FirstName,
            LastName = request.RegisterDto.LastName,
            Email = request.RegisterDto.Email,
            Phone = request.RegisterDto.Phone,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = passwordHash
        };

        // 4. Guardar en Base de Datos
        await unitOfWork.Repository<User>().Insert(newUser);
        await unitOfWork.CompleteAsync(cancellationToken);

        // 5. Generar Token (Auto-Login al registrarse)
        var token = tokenGenerator.GenerateToken(newUser);

        return new AuthResponseDto(newUser.Id, newUser.Email, token);
    }
}
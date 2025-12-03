using ArcheryAcademy.Application.DTOs.AuthDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Domain.Ports.Authentication;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.AuthUseCases.Command;

// El comando recibe el DTO
public record LoginCommand(LoginDto LoginDto) : IRequest<AuthResponseDto?>;

internal sealed class LoginCommandHandler(
    IUnitOfWork unitOfWork, 
    IJwtTokenGenerator tokenGenerator,
    IPasswordHasher passwordHasher)
    : IRequestHandler<LoginCommand, AuthResponseDto?>
{
    public async Task<AuthResponseDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Buscar usuario por Email
        var users = await unitOfWork.Repository<User>().FindWithIncludesAsync(
            u => u.Email == request.LoginDto.Email,
            "UserRoles", "UserRoles.Role" // Traemos la tabla intermedia y el Rol
        );
        
        var user = users.FirstOrDefault();
        
        if (user == null) return null;
        
        // Validar contraseña (Hash)
        bool isPasswordValid = passwordHasher.Verify(request.LoginDto.Password, user.PasswordHash);
        if (!isPasswordValid) return null;

        // 3. Generar token
        var token = tokenGenerator.GenerateToken(user);

        return new AuthResponseDto(user.Id, user.Email, token);
    }
}
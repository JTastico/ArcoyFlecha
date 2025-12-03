using ArcheryAcademy.Application.DTOs.AuthDto;
using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Domain.Ports.Authentication;
using MediatR;

namespace ArcheryAcademy.Application.UseCases.AuthUseCases.Command;

// El comando recibe el DTO
public record LoginCommand(LoginDto LoginDto) : IRequest<AuthResponseDto?>;

internal sealed class LoginCommandHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator tokenGenerator)
    : IRequestHandler<LoginCommand, AuthResponseDto?>
{
    public async Task<AuthResponseDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Buscar usuario por Email
        var user = await unitOfWork.Repository<User>()
            .FirstOrDefaultAsync(u => u.Email == request.LoginDto.Email, cancellationToken);

        if (user == null) return null;

        // 2. Validar password (texto plano para el demo)
        if (user.PasswordHash != request.LoginDto.Password) 
            return null;

        // 3. Generar token
        var token = tokenGenerator.GenerateToken(user);

        return new AuthResponseDto(user.Id, user.Email, token);
    }
}
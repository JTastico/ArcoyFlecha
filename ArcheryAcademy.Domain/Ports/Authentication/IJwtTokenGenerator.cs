using ArcheryAcademy.Domain.Entities;

namespace ArcheryAcademy.Domain.Ports.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
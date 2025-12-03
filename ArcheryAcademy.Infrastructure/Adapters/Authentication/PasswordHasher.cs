using ArcheryAcademy.Domain.Ports.Authentication;
using BCrypt.Net;

namespace ArcheryAcademy.Infrastructure.Adapters.Authentication;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
using ArcheryAcademy.Domain.Entities;

namespace ArcheryAcademy.Domain.Ports;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByIdWithRolesAsync(Guid id);
    
}
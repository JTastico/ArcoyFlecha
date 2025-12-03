using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArcheryAcademy.Infrastructure.Adapters.Repositories;

public class UserRepository(ArcheryAcademyDbContext context) : GenericRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByIdWithRolesAsync(Guid id)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role) // 👈 Esto carga los roles
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}
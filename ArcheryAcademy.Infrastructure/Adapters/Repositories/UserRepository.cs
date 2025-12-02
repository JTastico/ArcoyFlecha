using ArcheryAcademy.Domain.Entities;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Infrastructure.Persistence;

namespace ArcheryAcademy.Infrastructure.Adapters.Repositories;

public class UserRepository(ArcheryAcademyDbContext context) : GenericRepository<User>(context), IUserRepository
{
    
}
using System.Collections;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Infrastructure.Persistence;

namespace ArcheryAcademy.Infrastructure.Adapters.Repositories;

public class UnitOfWork: IUnitOfWork
{
    private readonly ArcheryAcademyDbContext  _context;
    private Hashtable? _repositories;
    
   //Repositorios especificos
   private IUserRepository? _userRepository;
   private IUserPlanRepository? _userPlanRepository;
   private IScheduleRepository? _scheduleRepository;
   private IBookingRepository? _bookingRepository;
   private IReportRepository? _reportRepository;
    public UnitOfWork(ArcheryAcademyDbContext  context)
    {
        _context = context;
    }

    //Repositorios especificos
    public IUserRepository UserRepository => 
        _userRepository ??= new UserRepository(_context);
    public IUserPlanRepository UserPlanRepository =>
        _userPlanRepository ??= new UserPlanRepository(_context);
    public IScheduleRepository ScheduleRepository =>
        _scheduleRepository ??= new ScheduleRepository(_context);
    public IBookingRepository BookingRepository =>
        _bookingRepository ??= new BookingRepository(_context);
    public IReportRepository ReportRepository =>
        _reportRepository ??= new ReportRepository(_context);
    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        // Si es la primera vez, inicializamos el diccionario
        if (_repositories == null)
        {
            _repositories = new Hashtable();
        }

        // Obtenemos el nombre del tipo de entidad como clave
        var type = typeof(TEntity).Name;

        // Si no existe en el diccionario, lo creamos
        if (!_repositories.ContainsKey(type))
        {
            // Tipo del repositorio genérico
            var repositoryType = typeof(GenericRepository<>);
            
            // Crear instancia del repositorio con reflexión
            var repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(typeof(TEntity)), 
                _context
            );

            // Guardamos en el diccionario para reutilizarlo
            _repositories.Add(type, repositoryInstance);
        }

        // Retornamos la instancia del repositorio
        return (IGenericRepository<TEntity>)_repositories[type]!;
    }

   
    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default )
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

 
    public void Dispose()
    {
        _context.Dispose();
    } 
    
}
namespace ArcheryAcademy.Domain.Ports;

public interface IUnitOfWork: IDisposable
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    IUserRepository UserRepository { get; }
    IUserPlanRepository UserPlanRepository { get; }
    IScheduleRepository ScheduleRepository { get; }
    IBookingRepository BookingRepository { get; }
    IReportRepository ReportRepository { get; }
    Task<int> CompleteAsync( CancellationToken cancellationToken = default);
}
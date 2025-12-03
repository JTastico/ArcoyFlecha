using System.Linq.Expressions;
using ArcheryAcademy.Domain.Ports;
using ArcheryAcademy.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArcheryAcademy.Infrastructure.Adapters.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly  ArcheryAcademyDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository( ArcheryAcademyDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task Insert(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }
    
    public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int page, 
        int pageSize, 
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "") // Ejemplo: "Roles,Instructor"
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        // 1. Aplicar Filtro
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // 2. Aplicar Includes (Relaciones)
        // Recorremos la cadena separada por comas y aplicamos .Include()
        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        // 3. Contar Total (Antes de paginar)
        var totalCount = await query.CountAsync();

        // 4. Ordenar
        if (orderBy != null)
        {
            query = orderBy(query);
        }

        // 5. Paginar
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
    
    public async Task<IEnumerable<T>> FindWithIncludesAsync(Expression<Func<T, bool>> predicate, params string[] includes)
    {
        IQueryable<T> query = _dbSet;

        // Agregamos cada relación solicitada (ej: "Booking", "User")
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        // Filtramos y ejecutamos
        return await query.Where(predicate).ToListAsync();
    }
}
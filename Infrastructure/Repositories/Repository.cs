using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Contexts;
using System.Linq.Expressions;
using Domain.Entities;
using System.Linq;

namespace Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbSet<T> _dbSet;

    #region Constructors
    public Repository(ApplicationDbContext context)
    {
        _dbSet = context.Set<T>();
    }
    #endregion

    #region Methods
    public async Task<IEnumerable<T>> GetWithPaginationAsync(
        Expression<Func<T, bool>> predicate,
        int pageIndex = 0,
        int pageSize = 10)
    {
        return await _dbSet
            .Where(predicate)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountAsync() => await _dbSet.CountAsync();

    public async Task<IEnumerable<T>> ToListAsync(CancellationToken cancellationToken = default)
        => await _dbSet.ToListAsync(cancellationToken);

    public async Task<T> FindAsync(object id, CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync(id, cancellationToken);

    public async Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(cancellationToken);

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

    public IEnumerable<T> Where(Expression<Func<T, bool>> expression)
        => _dbSet.Where(expression);

    public void Add(T entity) => _dbSet.Add(entity);

    public void AddRange(IEnumerable<T> entities) => _dbSet.AddRange(entities);

    public void Remove(T entity) => _dbSet.Remove(entity);

    public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

    public void Update(T entity) => _dbSet.Update(entity);

    public void UpdateRange(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);

    public IQueryable<T> AsNoTracking() => _dbSet.AsNoTracking();

    public IQueryable<T> AsQueryable() => _dbSet.AsQueryable();
    #endregion
}

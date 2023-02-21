using System.Linq.Expressions;

namespace Domain.Repositories;

public interface IRepository<T> 
    where T : class
{
    Task<IEnumerable<T>> GetWithPaginationAsync(
        Expression<Func<T, bool>> predicate,
        int pageIndex = 0,
        int pageSize = 10);

    Task<int> CountAsync();

    Task<T> FindAsync(object id, CancellationToken cancellationToken = default);

    Task<T> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> ToListAsync(CancellationToken cancellationToken = default);

    IEnumerable<T> Where(Expression<Func<T, bool>> expression);

    void Add(T entity);

    void AddRange(IEnumerable<T> entities);

    void Remove(T entity);

    void RemoveRange(IEnumerable<T> entities);

    void Update(T entity);

    void UpdateRange(IEnumerable<T> entities);

    IQueryable<T> AsNoTracking();

    IQueryable<T> AsQueryable();
}

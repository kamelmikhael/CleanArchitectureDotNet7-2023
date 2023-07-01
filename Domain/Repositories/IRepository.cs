using Domain.Common;
using System.Linq.Expressions;

namespace Domain.Repositories;

public interface IRepository<TEntity, TKey> 
    where TEntity : BaseEntity<TKey>
{
    Task<IEnumerable<TEntity>> GetWithPaginationAsync(
        Expression<Func<TEntity, bool>> predicate,
        int pageIndex = 0,
        int pageSize = 10);

    Task<int> CountAsync();

    Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default);

    Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> ToListAsync(CancellationToken cancellationToken = default);

    IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    IQueryable<TEntity> AsNoTracking();

    IQueryable<TEntity> AsQueryable();
}

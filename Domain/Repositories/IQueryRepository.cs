using Domain.Common;
using System.Linq.Expressions;

namespace Domain.Repositories;

public interface IQueryRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
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

    IQueryable<TEntity> AsNoTracking();

    IQueryable<TEntity> AsQueryable();
}

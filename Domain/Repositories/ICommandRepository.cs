using Domain.Common;
using System.Linq.Expressions;

namespace Domain.Repositories;

public interface ICommandRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
{
    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);
}
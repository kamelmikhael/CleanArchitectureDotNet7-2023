using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Contexts;
using System.Linq.Expressions;
using Infrastructure.Specifications;
using AutoMapper;
using Application.Extensions;
using Domain.Common;

namespace Infrastructure.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> 
    where TEntity : BaseEntity<TKey>
{
    protected readonly DbSet<TEntity> _dbSet;

    #region Constructors
    public Repository(ApplicationDbContext context)
    {
        _dbSet = context.Set<TEntity>();
    }
    #endregion

    #region Methods
    public async Task<IEnumerable<TEntity>> GetWithPaginationAsync(
        Expression<Func<TEntity, bool>> predicate,
        int pageIndex = 0,
        int pageSize = 10)
        => await _dbSet.Where(predicate)
            .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

    public async Task<int> CountAsync() => await _dbSet.CountAsync();

    public async Task<IEnumerable<TEntity>> ToListAsync(CancellationToken cancellationToken = default)
        => await _dbSet.ToListAsync(cancellationToken);

    public async Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync(id, cancellationToken);

    public async Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(cancellationToken);

    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

    public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        => _dbSet.Where(expression);

    public void Add(TEntity entity) => _dbSet.Add(entity);

    public void AddRange(IEnumerable<TEntity> entities) => _dbSet.AddRange(entities);

    public void Remove(TEntity entity) => _dbSet.Remove(entity);

    public void RemoveRange(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

    public void Update(TEntity entity) => _dbSet.Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

    public IQueryable<TEntity> AsNoTracking() => _dbSet.AsNoTracking();

    public IQueryable<TEntity> AsQueryable() => _dbSet.AsQueryable();

    public IQueryable<TEntity> ApplySpecification(Specification<TEntity, TKey> specification)
        => SpecificationEvaluator.GetQuery(_dbSet, specification);
    #endregion
}

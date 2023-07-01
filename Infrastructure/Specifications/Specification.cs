using Domain.Common;
using System.Linq.Expressions;

namespace Infrastructure.Specifications;

public abstract class Specification<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    protected Specification(Expression<Func<TEntity, bool>>? criteria)
        => Criteria = criteria;

    public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; private set; } = new();

    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }

    public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }

    public bool IsSplitQuery { get; private set; }
    public bool IsNoTracking { get; private set; }

    public bool IsPagedResult { get; private set; }
    public int PageIndex { get; private set; } = 0;
    public int PageSize { get; private set; } = 10;

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) 
        => IncludeExpressions.Add(includeExpression);

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        => OrderByExpression = orderByExpression;

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        => OrderByDescendingExpression = orderByDescendingExpression;

    protected void WithPaging(int pageIndex, int pageSize)
        => (IsPagedResult, PageIndex, PageSize) = (true, pageIndex, pageSize);

    protected void AsNoTracking() 
        => IsNoTracking = true;

    protected void AsSplitQuery() 
        => IsSplitQuery = true;
}

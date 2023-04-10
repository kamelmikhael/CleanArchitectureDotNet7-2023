using System.Linq.Expressions;

namespace Infrastructure.Specifications;

public abstract class Specification<TEntity>
    where TEntity : class
{
    protected Specification(Expression<Func<TEntity, bool>>? criteria)
        => Criteria = criteria;

    public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; private set; } = new();

    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }

    public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }

    public bool IsSplitQuery { get; protected set; }

    public bool IsPagedResult { get; protected set; }
    public int PageIndex { get; protected set; } = 0;
    public int PageSize { get; protected set; } = 10;

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) 
        => IncludeExpressions.Add(includeExpression);

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        => OrderByExpression = orderByExpression;

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        => OrderByDescendingExpression = orderByDescendingExpression;
}

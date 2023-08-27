using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Specifications;

internal static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity, TKey>(
        IQueryable<TEntity> inputQueryable,
        Specification<TEntity, TKey> specification)
        where TEntity : Entity<TKey>
    {
        IQueryable<TEntity> queryable = inputQueryable;

        if(specification.IsSplitQuery)
        {
            queryable = queryable.AsSplitQuery();
        }

        if (specification.IsNoTracking)
        {
            queryable = queryable.AsNoTracking();
        }

        if (specification.Criteria is not null)
        {
            queryable = queryable.Where(specification.Criteria);
        }

        queryable = specification.IncludeExpressions.Aggregate(
            queryable,
            (current, includeExpression) => queryable = current.Include(includeExpression));

        if (specification.OrderByExpression is not null)
        {
            queryable = queryable.OrderBy(specification.OrderByExpression);
        }
        else if (specification.OrderByDescendingExpression is not null)
        {
            queryable = queryable.OrderByDescending(specification.OrderByDescendingExpression);
        }

        if(specification.PageIndex.HasValue && specification.PageSize.HasValue)
        {
            queryable = queryable
                .Skip(specification.PageIndex.Value * specification.PageSize.Value)
                .Take(specification.PageSize.Value);
        }

        return queryable;
    }
}

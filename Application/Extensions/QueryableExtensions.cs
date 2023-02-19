using System.Linq.Expressions;

namespace Application.Extensions;

public static class QueryableExtensions
{
    //
    // Summary:
    //     Filters a System.Linq.IQueryable`1 by given predicate if given condition is true.
    //
    // Parameters:
    //   query:
    //     Queryable to apply filtering
    //
    //   condition:
    //     A boolean value
    //
    //   predicate:
    //     Predicate to filter the query
    //
    // Returns:
    //     Filtered or not filtered query based on condition
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition
        , Expression<Func<T, bool>> predicate)
    {
        if (!condition)
        {
            return query;
        }

        return query.Where(predicate);
    }
}

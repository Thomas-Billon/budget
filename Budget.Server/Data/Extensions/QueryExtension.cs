using Budget.Server.Core.Enums;
using System.Linq.Expressions;

namespace Budget.Server.Data.Extensions
{
    public static class QueryExtension
    {
        #region SortBy

        public static IOrderedQueryable<TSource> SortBy<TSource, TKey>(this IQueryable<TSource> query, Expression<Func<TSource, TKey>> expression)
            where TSource : class
        {
            return query.SortBy(SortDirection.Ascending, expression);
        }

        public static IOrderedQueryable<TSource> SortByDescending<TSource, TKey>(this IQueryable<TSource> query, Expression<Func<TSource, TKey>> expression)
            where TSource : class
        {
            return query.SortBy(SortDirection.Descending, expression);
        }

        public static IOrderedQueryable<TSource> SortBy<TSource, TKey>(this IQueryable<TSource> query, SortDirection direction, Expression<Func<TSource, TKey>> expression)
            where TSource : class
        {
            switch (direction)
            {
                case SortDirection.Ascending:
                    if (query is IOrderedQueryable<TSource> orderedQueryAsc)
                    {
                        return orderedQueryAsc.ThenBy(expression);
                    }
                    else
                    {
                        return query.OrderBy(expression);
                    }

                case SortDirection.Descending:
                    if (query is IOrderedQueryable<TSource> orderedQueryDesc)
                    {
                        return orderedQueryDesc.ThenByDescending(expression);
                    }
                    else
                    {
                        return query.OrderByDescending(expression);
                    }

                default:
                    throw new ArgumentException("Invalid sort direction", nameof(direction));
            }
        }

        #endregion SortBy

        #region SkipTake

        public const int EXTRA_ENTRY_TO_CHECK_IF_LAST_PAGE = 1;

        public static IQueryable<TSource> SkipTake<TSource>(this IQueryable<TSource> query, int skip, int take, bool isPaginationEnabled = false)
            where TSource : class
        {
            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            if (take > 0)
            {
                if (isPaginationEnabled)
                {
                    take += EXTRA_ENTRY_TO_CHECK_IF_LAST_PAGE;
                }

                query = query.Take(take);
            }

            return query;
        }

        #endregion SkipTake
    }
}

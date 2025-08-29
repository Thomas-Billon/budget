namespace Budget.Server.Core.Helpers
{
    public class Pagination<T>
            where T : class
    {
        public required List<T> Page { get; set; }
        public required bool IsLastPage { get; set; }

        public static Pagination<T> CreateFromQueryResult(List<T> items, int take)
        {
            var isLastPage = true;

            if (take != 0)
            {
                isLastPage = items.Count <= take;
                if (!isLastPage)
                {
                    items.RemoveAt(items.Count - PaginationExtension.EXTRA_ENTRY_TO_CHECK_IF_LAST_PAGE);
                }
            }

            return new Pagination<T>
            {
                Page = items,
                IsLastPage = isLastPage
            };
        }
    }

    public static class PaginationExtension
    {
        public const int EXTRA_ENTRY_TO_CHECK_IF_LAST_PAGE = 1;

        public static IQueryable<T> ApplyPaginationToQuery<T>(this IQueryable<T> query, int skip, int take)
            where T : class
        {
            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            if (take > 0)
            {
                query = query.Take(take + EXTRA_ENTRY_TO_CHECK_IF_LAST_PAGE);
            }

            return query;
        }
    }
}

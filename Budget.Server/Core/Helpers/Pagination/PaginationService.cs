namespace Budget.Server.Core.Helpers.Pagination
{
    public class PaginationService
    {
        const int EXTRA_ENTRY_TO_CHECK_IF_LAST_PAGE = 1;

        public IQueryable<T> ApplyPaginationToQuery<T>(IQueryable<T> query, int skip, int take)
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

        public Pagination<T> CreatePagination<T>(List<T> items, int take)
            where T : class
        {
            var isLastPage = true;

            if (take != 0)
            {
                isLastPage = items.Count <= take;
                if (!isLastPage)
                {
                    items.RemoveAt(items.Count - EXTRA_ENTRY_TO_CHECK_IF_LAST_PAGE);
                }
            }

            return new Pagination<T>
            {
                Page = items,
                IsLastPage = isLastPage
            };
        }
    }
}

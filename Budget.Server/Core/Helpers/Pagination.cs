namespace Budget.Server.Core.Helpers
{
    public class Pagination<T>
        where T : class
    {
        public required List<T> Page { get; set; }
        public required bool IsLastPage { get; set; }
    }

    public static class PaginationExtension
    {
        public static Pagination<T> ToPagination<T>(this List<T> items, int take)
            where T : class
        {
            var isLastPage = true;

            if (take != 0)
            {
                isLastPage = items.Count <= take;
                if (!isLastPage)
                {
                    items.RemoveRange(take, items.Count - take);
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

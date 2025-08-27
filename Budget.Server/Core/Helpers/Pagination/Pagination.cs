namespace Budget.Server.Core.Helpers.Pagination
{
    public class Pagination<T>
        where T : class
    {
        public required List<T> Page { get; set; }
        public required bool IsLastPage { get; set; }
    }
}

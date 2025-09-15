namespace Budget.Server.Data.Categories
{
    public static class CategoryExtension
    {
        public static IQueryable<Category> Where_IsRoot(this IQueryable<Category> query)
        {
            return query.Where(x => x.ParentCategoryId == null);
        }
    }
}

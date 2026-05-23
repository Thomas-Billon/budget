using Budget.Server.Core.Enums;
using Budget.Server.Data.Categories;
using System.Linq.Expressions;

namespace Budget.Server.Core.Categories
{
    public class CategoryQuery
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required CategoryColor Color { get; set; }
    }

    public static class CategoryQueryExtension
    {
        public static CategoryQuery ToQuery(this Category x)
        {
            return new CategoryQuery
            {
                Id = x.Id,
                Name = x.Name,
                Color = x.Color,
            };
        }
    }

    public class CategoryQueryOptions
    {
        public required CategoryQuery Base { get; set; }

        public static Expression<Func<Category, CategoryQueryOptions>> Select => c => new()
        {
            Base = c.ToQuery(),
        };
    }

    public class CategoryQueryHierarchy
    {
        public required CategoryQuery Base { get; set; }
        public required int? ParentCategoryId { get; set; }

        // Needs to be set manually after querying, as EF Core does not support recursive queries.
        public List<CategoryQueryHierarchy> SubCategories { get; set; } = [];

        public static Expression<Func<Category, CategoryQueryHierarchy>> Select => c => new()
        {
            Base = c.ToQuery(),
            ParentCategoryId = c.ParentCategoryId,
        };
    }

    public class CategoryQueryBalance
    {
        public required CategoryQuery Base { get; set; }

        public static Expression<Func<Category, CategoryQueryBalance>> Select => c => new()
        {
            Base = c.ToQuery(),
        };
    }

    public class CategoryQueryDetails
    {
        public required CategoryQuery Base { get; set; }
        public required int? ParentCategoryId { get; set; }
        public required List<CategoryQuery> SubCategories { get; set; }

        public static Expression<Func<Category, CategoryQueryDetails>> Select => c => new()
        {
            Base = c.ToQuery(),
            ParentCategoryId = c.ParentCategoryId,
            SubCategories = c.SubCategories.Select(sc => sc.ToQuery()).ToList(),
        };
    }
}

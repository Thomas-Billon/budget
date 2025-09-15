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

        public static Expression<Func<Category, CategoryQuery>> Select => x => x.ToQuery();
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

    public class CategoryQueryAll
    {
        public required CategoryQuery Base { get; set; }

        public static Expression<Func<Category, CategoryQueryAll>> Select => c => new()
        {
            Base = c.ToQuery(),
        };
    }

    public class CategoryQueryById
    {
        public required CategoryQuery Base { get; set; }
        public required List<CategoryQuery> SubCategories { get; set; }

        public static Expression<Func<Category, CategoryQueryById>> Select => c => new()
        {
            Base = c.ToQuery(),
            SubCategories = c.SubCategories.Select(sc => sc.ToQuery()).ToList(),
        };
    }
}

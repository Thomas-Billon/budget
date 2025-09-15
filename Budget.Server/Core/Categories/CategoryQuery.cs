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

        public static Expression<Func<Category, CategoryQuery>> Select => x => new CategoryQuery
        {
            Id = x.Id,
            Name = x.Name,
            Color = x.Color,
        };
    }
}

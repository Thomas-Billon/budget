using Budget.Server.Core.Categories.Enums;
using Budget.Server.Core.Transactions.Models;
using Budget.Server.Data.Categories;
using System.Linq.Expressions;

namespace Budget.Server.Core.Categories.Models
{
    public class CategoryQuery
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required CategoryColor Color { get; set; }

        public static Expression<Func<Category, CategoryQuery>> Select => c => c.ToQuery();
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

    public class CategoryQueryList
    {
        public required CategoryQuery Base { get; set; }
        public required int TransactionCount { get; set; }

        public static Expression<Func<Category, CategoryQueryList>> Select => c => new()
        {
            Base = c.ToQuery(),
            TransactionCount = c.Transactions.Count,
        };
    }

    public class CategoryQueryDetails
    {
        public required CategoryQuery Base { get; set; }
        public required List<TransactionQuery> Transactions { get; set; }

        public static Expression<Func<Category, CategoryQueryDetails>> Select => c => new()
        {
            Base = c.ToQuery(),
            Transactions = c.Transactions.Select(t => t.ToQuery()).ToList(),
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

    public class CategoryQueryFieldOptions
    {
        public required CategoryQuery Base { get; set; }

        public static Expression<Func<Category, CategoryQueryFieldOptions>> Select => c => new()
        {
            Base = c.ToQuery(),
        };
    }
}

using Budget.Server.Core.Categories;
using Budget.Server.Core.Enums;
using Budget.Server.Data.Transactions;
using System.Linq.Expressions;

namespace Budget.Server.Core.Transactions
{
    public class TransactionQuery
    {
        public required int Id { get; set; }
        public required TransactionType Type { get; set; }
        public required decimal Amount { get; set; }
        public required string Reason { get; set; }
        public required DateOnly Date { get; set; }
        public required PaymentMethod PaymentMethod { get; set; }
        public required string Comment { get; set; }
    }

    public static class TransactionQueryExtension
    {
        public static TransactionQuery ToQuery(this Transaction x)
        {
            return new TransactionQuery
            {
                Id = x.Id,
                Type = x.Type,
                Amount = x.Amount,
                Reason = x.Reason,
                Date = x.Date,
                PaymentMethod = x.PaymentMethod,
                Comment = x.Comment,
            };
        }
    }

    public class TransactionQuery_History
    {
        public required TransactionQuery Base { get; set; }
        public required List<CategoryQuery> Categories { get; set; }

        public static Expression<Func<Transaction, TransactionQuery_History>> Select => t => new()
        {
            Base = t.ToQuery(),
            Categories = t.Categories.Select(c => c.ToQuery()).ToList(),
        };
    }

    public class TransactionQuery_Details
    {
        public required TransactionQuery Base { get; set; }
        public required List<CategoryQuery> Categories { get; set; }

        public static Expression<Func<Transaction, TransactionQuery_Details>> Select => t => new()
        {
            Base = t.ToQuery(),
            Categories = t.Categories.Select(c => c.ToQuery()).ToList(),
        };
    }
}

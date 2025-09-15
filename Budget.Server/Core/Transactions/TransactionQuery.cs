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
        public required double Amount { get; set; }
        public required string Reason { get; set; }
        public required DateOnly? Date { get; set; }
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

    public class TransactionQueryList
    {
        public required TransactionQuery Base { get; set; }

        public static Expression<Func<Transaction, TransactionQueryList>> Select => t => new()
        {
            Base = t.ToQuery(),
        };
    }

    public class TransactionQueryById
    {
        public required TransactionQuery Base { get; set; }
        public required List<CategoryQuery> Categories { get; set; }

        public static Expression<Func<Transaction, TransactionQueryById>> Select => t => new()
        {
            Base = t.ToQuery(),
            Categories = t.Categories.Select(c => c.ToQuery()).ToList(),
        };
    }
}

using Budget.Server.Core.Categories.Models;
using Budget.Server.Core.Transactions.Enums;
using Budget.Server.Core.Transactions.Models;
using Budget.Server.Data.Transactions;
using System.Linq.Expressions;

namespace Budget.Server.Core.Transactions.Models
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
        public required int AccountId { get; set; }
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
                AccountId = x.AccountId,
            };
        }
    }

    public class TransactionQueryHistory
    {
        public required TransactionQuery Base { get; set; }
        public required List<CategoryQuery> Categories { get; set; }

        public static Expression<Func<Transaction, TransactionQueryHistory>> Select => t => new()
        {
            Base = t.ToQuery(),
            Categories = t.Categories.Select(c => c.ToQuery()).ToList(),
        };
    }

    public class TransactionQueryDetails
    {
        public required TransactionQuery Base { get; set; }
        public required List<CategoryQuery> Categories { get; set; }

        public static Expression<Func<Transaction, TransactionQueryDetails>> Select => t => new()
        {
            Base = t.ToQuery(),
            Categories = t.Categories.Select(c => c.ToQuery()).ToList(),
        };
    }

    public class TransactionQueryStats
    {
        public required TransactionQuery Base { get; set; }
        public required List<CategoryQuery> Categories { get; set; }

        public static Expression<Func<Transaction, TransactionQueryStats>> Select => t => new()
        {
            Base = t.ToQuery(),
            Categories = t.Categories.Select(c => c.ToQuery()).ToList(),
        };
    }
}

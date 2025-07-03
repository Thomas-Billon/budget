using Budget.Server.Entities;
using Budget.Server.Enums;
using System.Linq.Expressions;

namespace Budget.Server.Queries
{
    public class TransactionQuery
    {
        public required int Id { get; set; }
        public required double Amount { get; set; }
        public DateOnly? Date { get; set; }
        public TransactionPaymentMethod? PaymentMethod { get; set; }
        public string? Comment { get; set; }

        public static Expression<Func<Transaction, TransactionQuery>> Select => x => new TransactionQuery
        {
            Id = x.Id,
            Amount = x.Amount,
            Date = x.Date,
            PaymentMethod = x.PaymentMethod,
            Comment = x.Comment
        };
    }

    public static class TransactionQueryExtension
    {
    }
}

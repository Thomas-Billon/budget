using Budget.Server.Core.Enums;
using System.Linq.Expressions;

namespace Budget.Server.Data.Transactions
{
    public class TransactionQuery
    {
        public required int Id { get; set; }
        public required TransactionType Type { get; set; }
        public required double Amount { get; set; }
        public required string Title { get; set; }
        public required DateOnly? Date { get; set; }
        public required PaymentMethod PaymentMethod { get; set; }
        public required string Comment { get; set; }

        public static Expression<Func<Transaction, TransactionQuery>> Select => x => new TransactionQuery
        {
            Id = x.Id,
            Type = x.Type,
            Amount = x.Amount,
            Title = x.Title,
            Date = x.Date,
            PaymentMethod = x.PaymentMethod,
            Comment = x.Comment
        };
    } 
}

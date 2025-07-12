using Budget.Server.Entities;
using Budget.Server.Enums;
using System.Linq.Expressions;

namespace Budget.Server.Dbos
{
    public class TransactionDBO
    {
        public required int Id { get; set; }
        public required TransactionType Type { get; set; }
        public required double Amount { get; set; }
        public required DateOnly? Date { get; set; }
        public required PaymentMethod PaymentMethod { get; set; }
        public required string Comment { get; set; }

        public static Expression<Func<Transaction, TransactionDBO>> Select => x => new TransactionDBO
        {
            Id = x.Id,
            Type = x.Type,
            Amount = x.Amount,
            Date = x.Date,
            PaymentMethod = x.PaymentMethod,
            Comment = x.Comment
        };
    }

    public static class TransactionDBOExtension
    {
    }
}

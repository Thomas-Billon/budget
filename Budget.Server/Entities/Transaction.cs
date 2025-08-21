using Budget.Server.Enums;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Entities
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public TransactionType Type { get; set; } = TransactionType.None;
        //public Account Account { get; set; } -> Currency will be handled inside the Account entity
        public double Amount { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public DateOnly? Date { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.None;
        public string Comment { get; set; } = string.Empty;

        // Merchant -> string or entity ?

        // public int? CategoryId { get; set; };
        // public Category? Category { get; set; };
    }
}

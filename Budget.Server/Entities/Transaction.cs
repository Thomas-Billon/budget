using Budget.Server.Enums;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Entities
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public TransactionType Type { get; set; } = TransactionType.None;
        public double Amount { get; set; } = 0;
        // public Currency? Currency { get; set; }
        public DateOnly? Date { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public string? Comment { get; set; }

        // Merchant -> string or entity ?

        // public int? CategoryId { get; set; };
        // public Category? Category { get; set; };
    }
}

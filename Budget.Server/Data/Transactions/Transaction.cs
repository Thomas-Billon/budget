using Budget.Server.Core.Enums;
using Budget.Server.Data.Categories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget.Server.Data.Transactions
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        public TransactionType Type { get; set; } = TransactionType.None;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } = 0;

        public string Reason { get; set; } = string.Empty;

        public DateOnly Date { get; set; }

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.None;

        public string Comment { get; set; } = string.Empty;

        //public Account Account { get; set; } -> Currency will be handled inside the Account entity

        // Merchant -> string or entity ?

        public List<Category> Categories { get; set; } = new List<Category>();
    }
}

using Budget.Server.Data.Categories;

namespace Budget.Server.Data.Transactions
{
    public class TransactionCategory
    {
        public int TransactionId { get; set; }
        public Transaction? Transaction { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}

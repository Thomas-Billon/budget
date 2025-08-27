using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Data.Transactions
{
    public class TransactionConfiguration
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Transaction>();

            entity.ToTable(nameof(ApplicationDbContext.Transactions));
        }
    }
}

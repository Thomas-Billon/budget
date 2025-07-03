using Budget.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Data.Configurations
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

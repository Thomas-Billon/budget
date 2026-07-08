using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Data.Transactions
{
    public class TransactionConfiguration
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Transaction>();

            entity.ToTable(nameof(ApplicationDbContext.Transactions));

            entity
                .HasMany(t => t.Categories)
                .WithMany(c => c.Transactions)
                .UsingEntity<TransactionCategory>(
                    right => right
                        .HasOne(tc => tc.Category)
                        .WithMany()
                        .HasForeignKey(tc => tc.CategoryId),
                    left => left
                        .HasOne(tc => tc.Transaction)
                        .WithMany()
                        .HasForeignKey(tc => tc.TransactionId),
                    join =>
                    {
                        join.ToTable(nameof(ApplicationDbContext.TransactionsCategories));
                        join.HasKey(tc => new { tc.TransactionId, tc.CategoryId });
                    });

            entity
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

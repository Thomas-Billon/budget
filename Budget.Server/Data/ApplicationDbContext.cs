using Budget.Server.Data.Categories;
using Budget.Server.Data.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Data
{
    public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            CategoryConfiguration.OnModelCreating(modelBuilder);
            TransactionConfiguration.OnModelCreating(modelBuilder);
        }
    }
}

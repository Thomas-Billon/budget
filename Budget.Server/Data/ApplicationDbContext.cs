using Budget.Server.Data.Configurations;
using Budget.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Data
{
    public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            TransactionConfiguration.OnModelCreating(modelBuilder);
        }
    }
}

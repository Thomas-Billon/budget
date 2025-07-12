using Budget.Server.Data;
using Budget.Server.Entities;
using Budget.Server.Enums;
using Budget.Server.Dbos;
using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Services
{
    public class TransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<TransactionDBO>> GetAll()
        {
            return _context.Transactions.AsNoTracking()
                .Select(TransactionDBO.Select)
                .ToListAsync();
        }

        public Task<TransactionDBO?> GetById(int id)
        {
            return _context.Transactions.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(TransactionDBO.Select)
                .FirstOrDefaultAsync();
        }

        public Task<int> Create(Transaction entity)
        {
            _context.Transactions.Add(entity);

            return _context.SaveChangesAsync();
        }

        public Task<int> Update(int id, Transaction entity)
        {
            return _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Amount, entity.Amount)
                    .SetProperty(x => x.Date, entity.Date)
                    .SetProperty(x => x.PaymentMethod, entity.PaymentMethod)
                    .SetProperty(x => x.Comment, entity.Comment)
                );
        }

        public Task<int> UpdateType(int id, TransactionType type)
        {
            return _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Type, type)
                );
        }

        public Task<int> UpdateAmount(int id, double amount)
        {
            return _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Amount, amount)
                );
        }

        public Task<int> UpdateDate(int id, DateOnly? date)
        {
            return _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Date, date)
                );
        }

        public Task<int> UpdatePaymentMethod(int id, PaymentMethod paymentMethod)
        {
            return _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.PaymentMethod, paymentMethod)
                );
        }

        public Task<int> UpdateComment(int id, string comment)
        {
            return _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Comment, comment)
                );
        }

        public Task<int> Delete(int id)
        {
            return _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}

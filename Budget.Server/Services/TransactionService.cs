using Budget.Server.Data;
using Budget.Server.Entities;
using Budget.Server.Dbos;
using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Services
{
    public class TransactionService
    {
        const int NO_DATABASE_OPERATION = 0;

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
                    .SetProperty(x => x.Type, entity.Type)
                    .SetProperty(x => x.Amount, entity.Amount)
                    .SetProperty(x => x.Title, entity.Title)
                    .SetProperty(x => x.Date, entity.Date)
                    .SetProperty(x => x.PaymentMethod, entity.PaymentMethod)
                    .SetProperty(x => x.Comment, entity.Comment)
                );
        }

        public async Task<int> UpdatePartial(int id, Transaction entity)
        {
            var dbEntity = await _context.Transactions
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (dbEntity == null)
            {
                return NO_DATABASE_OPERATION;
            }

            dbEntity.Type = entity.Type;
            dbEntity.Amount = entity.Amount;
            dbEntity.Title = entity.Title;
            dbEntity.Date = entity.Date;
            dbEntity.PaymentMethod = entity.PaymentMethod;
            dbEntity.Comment = entity.Comment;

            var changes = _context.Entry(dbEntity).Properties.Count(x => x.IsModified);

            if (changes > 0)
            {
                await _context.SaveChangesAsync();
            }

            return changes;
        }

        public Task<int> Delete(int id)
        {
            return _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}

using Budget.Server.Data;
using Budget.Server.Entities;
using Budget.Server.Enums;
using Budget.Server.Queries;
using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Services
{
    public class TransactionService : CustomServiceBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<TransactionQuery>> GetAll()
        {
            return await _context.Transactions.AsNoTracking()
                .Select(TransactionQuery.Select)
                .ToListAsync();
        }

        public Task<TransactionQuery?> GetById(int id)
        {
            return _context.Transactions.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(TransactionQuery.Select)
                .FirstOrDefaultAsync();
        }

        public async Task<Transaction> Create(Transaction entity)
        {
            Validate(entity, nameof(Create));

            _context.Transactions.Add(entity);

            await _context.SaveChangesAsync();

            EnsureCreated(entity);

            return entity;
        }

        public async Task<Transaction> Update(int id, Transaction entity)
        {
            Validate(entity, nameof(Update));

            var updateCount = await _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Amount, entity.Amount)
                    .SetProperty(x => x.Date, entity.Date)
                    .SetProperty(x => x.PaymentMethod, entity.PaymentMethod)
                    .SetProperty(x => x.Comment, entity.Comment)
                );

            EnsureUpdated(updateCount);

            return entity;
        }

        public async Task<double> UpdateAmount(int id, double amount)
        {
            if (!IsAmountValid(amount))
            {
                throw new ArgumentException(nameof(amount), "Parameter is invalid for update");
            }

            var updateCount = await _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Amount, amount)
                );

            EnsureUpdated(updateCount);

            return amount;
        }

        public async Task<DateOnly?> UpdateDate(int id, DateOnly? date)
        {
            if (!IsDateValid(date))
            {
                throw new ArgumentException(nameof(date), "Parameter is invalid for update");
            }

            var updateCount = await _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Date, date)
                );

            EnsureUpdated(updateCount);

            return date;
        }

        public async Task<PaymentMethod?> UpdatePaymentMethod(int id, PaymentMethod? paymentMethod)
        {
            if (!IsPaymentMethodValid(paymentMethod))
            {
                throw new ArgumentException(nameof(paymentMethod), "Parameter is invalid for update");
            }

            var updateCount = await _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.PaymentMethod, paymentMethod)
                );

            EnsureUpdated(updateCount);

            return paymentMethod;
        }

        public async Task<string?> UpdateComment(int id, string? comment)
        {
            if (!IsCommentValid(comment))
            {
                throw new ArgumentException(nameof(comment), "Parameter is invalid for update");
            }

            var updateCount = await _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Comment, comment)
                );

            EnsureUpdated(updateCount);

            return comment;
        }

        public async Task Delete(int id)
        {
            var deleteCount = await _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            EnsureDeleted(deleteCount);
        }

        private void Validate(Transaction entity, string operation)
        {
            if (!IsValid(entity))
            {
                throw new ArgumentException(nameof(entity), $"Parameter is invalid for {operation}");
            }
        }

        private bool IsValid(Transaction entity) => IsAmountValid(entity.Amount) &&
            IsDateValid(entity.Date) &&
            IsPaymentMethodValid(entity.PaymentMethod) &&
            IsCommentValid(entity.Comment);

        private bool IsAmountValid(double amount) => true;

        private bool IsDateValid(DateOnly? date) => true;

        private bool IsPaymentMethodValid(PaymentMethod? paymentMethod) => true;

        private bool IsCommentValid(string? comment) => true;

        private void EnsureCreated(Transaction entity)
        {
            if (entity == null)
            {
                throw new DbUpdateException("No entries were created");
            }
        }

        private void EnsureUpdated(int updateCount)
        {
            if (updateCount != 1)
            {
                throw new DbUpdateException("No entries were updated");
            }
        }

        private void EnsureDeleted(int deleteCount)
        {
            if (deleteCount != 1)
            {
                throw new DbUpdateException("No entries were deleted");
            }
        }
    }
}

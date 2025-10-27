using Budget.Server.Core.Enums;
using Budget.Server.Data;
using Budget.Server.Data.Extensions;
using Budget.Server.Data.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Core.Transactions
{
    public class TransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService
        (
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        public Task<List<TransactionQuery_History>> GetTransactionHistory(TransactionHistoryParameters parameters)
        {
            var query = _context.Transactions.AsNoTracking();

            query = parameters.Filter.Type switch
            {
                TransactionType.Income => query.Where_IsIncome(),
                TransactionType.Expense => query.Where_IsExpense(),
                _ => query
            };

            if (parameters.Filter.DateRange != null && parameters.Filter.DateRange.IsCustom)
            {
                if (parameters.Filter.DateRange.StartDate != null)
                {
                    query = query.Where_IsAfterOrOnDate(parameters.Filter.DateRange.StartDate.Value);
                }
                if (parameters.Filter.DateRange.EndDate != null)
                {
                    query = query.Where_IsBeforeOrOnDate(parameters.Filter.DateRange.EndDate.Value);
                }
            }
            else
            {
                query = parameters.Filter.DateRange?.Preset switch
                {
                    DateRangePreset.Last7Days => query.Where_IsInLast7Days(),
                    DateRangePreset.Last30Days => query.Where_IsInLast30Days(),
                    DateRangePreset.ThisMonth => query.Where_IsInThisMonth(),
                    DateRangePreset.LastMonth => query.Where_IsInLastMonth(),
                    DateRangePreset.ThisYear => query.Where_IsInThisYear(),
                    DateRangePreset.LastYear => query.Where_IsInLastYear(),
                    _ => query
                };
            }

            foreach (var (key, direction) in parameters.Sort)
            {
                query = key switch
                {
                    nameof(Transaction.Amount) => query.OrderBy_Amount(direction),
                    nameof(Transaction.Date) => query.OrderBy_Date(direction),
                    _ => query,
                };
            }

            query = query.SkipTake(parameters.Skip, parameters.Take, parameters.IsPaginationEnabled);

            return query.Select(TransactionQuery_History.Select)
                .ToListAsync();
        }

        public Task<TransactionQuery_Details?> GetTransactionDetails(int id)
        {
            return _context.Transactions.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(TransactionQuery_Details.Select)
                .FirstOrDefaultAsync();
        }

        public Task<int> CreateTransaction(TransactionCreateParameters parameters)
        {
            var entity = new Transaction
            {
                Type = parameters.Request.Type,
                Amount = parameters.Request.Amount,
                Reason = parameters.Request.Reason,
                Date = parameters.Request.Date,
                PaymentMethod = parameters.Request.PaymentMethod,
                Comment = parameters.Request.Comment,
            };

            // Categories

            _context.Transactions.Add(entity);

            return _context.SaveChangesAsync();
        }

        public Task<int> UpdateTransaction(int id, TransactionUpdateParameters parameters)
        {
            return _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Type, parameters.Request.Type)
                    .SetProperty(x => x.Amount, parameters.Request.Amount)
                    .SetProperty(x => x.Reason, parameters.Request.Reason)
                    .SetProperty(x => x.Date, parameters.Request.Date)
                    .SetProperty(x => x.PaymentMethod, parameters.Request.PaymentMethod)
                    .SetProperty(x => x.Comment, parameters.Request.Comment)
                );

            // Categories
        }

        public async Task<int> PatchTransaction(int id, TransactionPatchParameters parameters)
        {
            var entity = await _context.Transactions.FindAsync(id);

            if (entity == null)
            {
                return 0;
            }

            if (parameters.Request.Type?.IsSet == true) entity.Type = parameters.Request.Type.Value;
            if (parameters.Request.Amount?.IsSet == true) entity.Amount = parameters.Request.Amount.Value;
            if (parameters.Request.Reason?.IsSet == true) entity.Reason = parameters.Request.Reason.Value ?? string.Empty;
            if (parameters.Request.Date?.IsSet == true) entity.Date = parameters.Request.Date.Value;
            if (parameters.Request.PaymentMethod?.IsSet == true) entity.PaymentMethod = parameters.Request.PaymentMethod.Value;
            if (parameters.Request.Comment?.IsSet == true) entity.Comment = parameters.Request.Comment.Value ?? string.Empty;

            // Categories

            return await _context.SaveChangesAsync();
        }

        public Task<int> DeleteTransaction(int id)
        {
            return _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}

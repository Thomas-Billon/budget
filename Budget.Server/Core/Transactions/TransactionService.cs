using Budget.Server.Api.Transactions.Models.Requests;
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

        public Task<List<TransactionQuery_History>> GetTransactionHistory(TransactionHistoryParameters args)
        {
            var query = _context.Transactions.AsNoTracking();

            query = args.Filter.Type switch
            {
                TransactionType.Income => query.Where_IsIncome(),
                TransactionType.Expense => query.Where_IsExpense(),
                _ => query
            };

            if (args.Filter.DateRange != null && args.Filter.DateRange.IsCustom)
            {
                if (args.Filter.DateRange.StartDate != null)
                {
                    query = query.Where_IsAfterOrOnDate(args.Filter.DateRange.StartDate.Value);
                }
                if (args.Filter.DateRange.EndDate != null)
                {
                    query = query.Where_IsBeforeOrOnDate(args.Filter.DateRange.EndDate.Value);
                }
            }
            else
            {
                query = args.Filter.DateRange?.Preset switch
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

            foreach (var (key, direction) in args.Sort)
            {
                query = key switch
                {
                    nameof(Transaction.Amount) => query.OrderBy_Amount(direction),
                    nameof(Transaction.Date) => query.OrderBy_Date(direction),
                    _ => query,
                };
            }

            query = query.SkipTake(args.Skip, args.Take, args.IsPaginationEnabled);

            return query.Select(TransactionQuery_History.Select)
                .ToListAsync();
        }

        public Task<List<TransactionQuery_History>> GetTransactionHistoryBetweenDates(DateOnly? startDate, DateOnly? endDate)
        {
            var query = _context.Transactions.AsNoTracking();

            if (startDate != null)
            {
                query = query.Where_IsAfterOrOnDate(startDate.Value);
            }

            if (endDate != null)
            {
                query = query.Where_IsBeforeOrOnDate(endDate.Value);
            }

            return query.Select(TransactionQuery_History.Select)
                .ToListAsync();
        }

        public Task<TransactionQueryById?> GetById(int id)
        {
            return _context.Transactions.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(TransactionQueryById.Select)
                .FirstOrDefaultAsync();
        }

        public Task<int> Create(TransactionCreateRequest request)
        {
            var entity = new Transaction
            {
                Type = request.Type,
                Amount = request.Amount,
                Reason = request.Reason,
                Date = request.Date,
                PaymentMethod = request.PaymentMethod,
                Comment = request.Comment,
            };

            _context.Transactions.Add(entity);

            return _context.SaveChangesAsync();
        }

        public Task<int> Update(int id, TransactionUpdateRequest request)
        {
            return _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Type, request.Type)
                    .SetProperty(x => x.Amount, request.Amount)
                    .SetProperty(x => x.Reason, request.Reason)
                    .SetProperty(x => x.Date, request.Date)
                    .SetProperty(x => x.PaymentMethod, request.PaymentMethod)
                    .SetProperty(x => x.Comment, request.Comment)
                );
        }

        public async Task<int> Patch(int id, TransactionPatchRequest request)
        {
            var entity = await _context.Transactions.FindAsync(id);

            if (entity == null)
            {
                return 0;
            }

            if (request.Type?.IsSet == true) entity.Type = request.Type.Value;
            if (request.Amount?.IsSet == true) entity.Amount = request.Amount.Value;
            if (request.Reason?.IsSet == true) entity.Reason = request.Reason.Value ?? string.Empty;
            if (request.Date?.IsSet == true) entity.Date = request.Date.Value;
            if (request.PaymentMethod?.IsSet == true) entity.PaymentMethod = request.PaymentMethod.Value;
            if (request.Comment?.IsSet == true) entity.Comment = request.Comment.Value ?? string.Empty;

            return await _context.SaveChangesAsync();
        }

        public Task<int> Delete(int id)
        {
            return _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}

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

        public Task<List<TransactionQuery_History>> GetTransactionHistory(TransactionQueryableOptions options)
        {
            return GetTransactions_AsQueryable(options).AsNoTracking()
                .Select(TransactionQuery_History.Select)
                .ToListAsync();
        }

        public Task<List<TransactionQuery_Balance>> GetTransactionBalance(TransactionQueryableOptions options)
        {
            return GetTransactions_AsQueryable(options).AsNoTracking()
                .Select(TransactionQuery_Balance.Select)
                .ToListAsync();
        }

        public Task<TransactionQuery_Details?> GetTransactionDetails(int id)
        {
            return GetTransactionById_AsQueryable(id).AsNoTracking()
                .Select(TransactionQuery_Details.Select)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateTransaction(TransactionCreateRequest request)
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

            // Categories
            await AddCategoriesToTransaction(entity, request.CategoryIds);

            _context.Transactions.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateTransaction(int id, TransactionUpdateRequest request)
        {
            var entity = await GetTransactionById_AsQueryable(id)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return 0;
            }

            entity.Type = request.Type;
            entity.Amount = request.Amount;
            entity.Reason = request.Reason;
            entity.Date = request.Date;
            entity.PaymentMethod = request.PaymentMethod;
            entity.Comment = request.Comment;

            // Categories
            await AddCategoriesToTransaction(entity, request.CategoryIds);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> PatchTransaction(int id, TransactionPatchRequest request)
        {
            var entity = await GetTransactionById_AsQueryable(id)
                .FirstOrDefaultAsync();

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

            // Categories
            if (request.CategoryIds?.IsSet == true)
            {
                await AddCategoriesToTransaction(entity, request.CategoryIds.Value ?? []);
            }

            return await _context.SaveChangesAsync();
        }

        public Task<int> DeleteTransaction(int id)
        {
            return _context.Transactions
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }

        #region Private

        #region Get data

        private IQueryable<Transaction> GetTransactions_AsQueryable(TransactionQueryableOptions options)
        {
            var query = _context.Transactions
                .Include(x => x.Categories)
                .Where_HasTypes(options.Filter.Types);

            if (options.Filter.DateRange.IsCustom)
            {
                if (options.Filter.DateRange.StartDate != null)
                {
                    query = query.Where_IsAfterOrOnDate(options.Filter.DateRange.StartDate.Value);
                }
                if (options.Filter.DateRange.EndDate != null)
                {
                    query = query.Where_IsBeforeOrOnDate(options.Filter.DateRange.EndDate.Value);
                }
            }
            else
            {
                query = options.Filter.DateRange.Preset switch
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

            foreach (var (key, direction) in options.Sort)
            {
                query = key switch
                {
                    nameof(Transaction.Amount) => query.OrderBy_Amount(direction),
                    nameof(Transaction.Date) => query.OrderBy_Date(direction),
                    _ => query,
                };
            }

            query = query.SkipTake(options.Skip, options.Take, options.IsPaginationEnabled);

            return query;
        }

        private IQueryable<Transaction> GetTransactionById_AsQueryable(int id)
        {
            return _context.Transactions
                .Include(x => x.Categories)
                .Where(x => x.Id == id);
        }

        #endregion Get data

        #region Categories

        private async Task AddCategoriesToTransaction(Transaction entity, List<int> categoryIds)
        {
            entity.Categories.Clear();

            if (categoryIds.Any() == true)
            {
                var categories = await _context.Categories
                    .Where(x => categoryIds.Contains(x.Id))
                    .ToListAsync();

                foreach (var category in categories)
                {
                    entity.Categories.Add(category);
                }
            }
        }

        #endregion Categories

        #endregion Private
    }
}

using Budget.Server.Api.Transactions.Models.Requests;
using Budget.Server.Core.Transactions.Models;
using Budget.Server.Data;
using Budget.Server.Data.Transactions;
using Microsoft.EntityFrameworkCore;
using Budget.Server.Core.Shared;

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

        public Task<List<TransactionQueryStats>> GetStats(TransactionQueryParameters parameters, string userId)
        {
            return GetByParams_AsQueryable(parameters, userId).AsNoTracking()
                .Select(TransactionQueryStats.Select)
                .ToListAsync();
        }

        public async Task<Pagination<TransactionQueryHistory>> GetHistory(TransactionQueryParameters parameters, string userId)
        {
            var transactions = await GetByParams_AsQueryable(parameters, userId).AsNoTracking()
                .Select(TransactionQueryHistory.Select)
                .ToListAsync();

            return transactions.ToPagination(parameters.Take);
        }

        #region CRUD

        public Task<TransactionQueryDetails?> GetDetails(int id, string userId)
        {
            return GetById_AsQueryable(id, userId).AsNoTracking()
                .Select(TransactionQueryDetails.Select)
                .FirstOrDefaultAsync();
        }

        public async Task<int> Create(TransactionCreateRequest request, string userId)
        {
            var entity = new Transaction
            {
                Type = request.Type,
                Amount = request.Amount,
                Reason = request.Reason,
                Date = request.Date,
                PaymentMethod = request.PaymentMethod,
                Comment = request.Comment,
                AccountId = request.AccountId,
                UserId = userId,
            };

            // Categories
            await AddCategoriesToTransaction(entity, request.CategoryIds);

            _context.Transactions.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(int id, TransactionUpdateRequest request, string userId)
        {
            var entity = await GetById_AsQueryable(id, userId)
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
            entity.AccountId = request.AccountId;

            // Categories
            await AddCategoriesToTransaction(entity, request.CategoryIds);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Patch(int id, TransactionPatchRequest request, string userId)
        {
            var entity = await GetById_AsQueryable(id, userId)
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
            if (request.AccountId?.IsSet == true) entity.AccountId = request.AccountId.Value;

            // Categories
            if (request.CategoryIds?.IsSet == true)
            {
                await AddCategoriesToTransaction(entity, request.CategoryIds.Value ?? []);
            }

            return await _context.SaveChangesAsync();
        }

        public Task<int> Delete(int id, string userId)
        {
            return GetById_AsQueryable(id, userId)
                .ExecuteDeleteAsync();
        }

        #endregion CRUD

        #region Queryable

        private IQueryable<Transaction> GetByParams_AsQueryable(TransactionQueryParameters parameters, string userId)
        {
            var query = _context.Transactions
                .Include(x => x.Categories)
                .Where(x => x.UserId == userId)
                .Where_HasTypes(parameters.Filter.Types)
                .Where_IsInDateRange(parameters.Filter.DateRange);

            // TODO: Refactorize this bit in one line
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

            return query;
        }

        private IQueryable<Transaction> GetById_AsQueryable(int id, string userId)
        {
            return _context.Transactions
                .Include(x => x.Categories)
                .Where(x => x.Id == id)
                .Where(x => x.UserId == userId);
        }

        #endregion Queryable

        #region Categories

        private async Task AddCategoriesToTransaction(Transaction entity, List<int> categoryIds)
        {
            entity.Categories.Clear();

            if (categoryIds.Any())
            {
                var categories = await _context.Categories
                    .Where(x => categoryIds.Contains(x.Id))
                    .Where(x => x.UserId == entity.UserId)
                    .ToListAsync();

                foreach (var category in categories)
                {
                    entity.Categories.Add(category);
                }
            }
        }

        #endregion Categories
    }
}

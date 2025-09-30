using Budget.Server.Api.Transactions.Models.Requests;
using Budget.Server.Core.Helpers;
using Budget.Server.Data;
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

        public async Task<Pagination<TransactionQueryList>> GetPaginatedList(int skip, int take, HashSet<TransactionFilterOption> filters, TransactionSortOption sort)
        {
            var query = _context.Transactions.AsNoTracking();

            foreach (var filter in filters)
            {
                query = filter switch
                {
                    TransactionFilterOption.Income => query.Where_IsIncome(),
                    TransactionFilterOption.Expense => query.Where_IsExpense(),
                    TransactionFilterOption.Last7Days => query.Where_IsInLast7Days(),
                    TransactionFilterOption.Last30Days => query.Where_IsInLast30Days(),
                    TransactionFilterOption.ThisMonth => query.Where_IsInThisMonth(),
                    TransactionFilterOption.ThisYear => query.Where_IsInThisYear(),
                    _ => query
                };
            }

            query = sort switch
            {
                TransactionSortOption.DateAsc => query.OrderBy(x => x.Date),
                TransactionSortOption.DateDesc => query.OrderByDescending(x => x.Date),
                TransactionSortOption.AmountAsc => query.OrderBy(x => x.Amount),
                TransactionSortOption.AmountDesc => query.OrderByDescending(x => x.Amount),
                _ => query
            };

            query = query.ApplyPaginationToQuery(skip, take);

            var entities = await query.Select(TransactionQueryList.Select)
                .ToListAsync();

            return Pagination<TransactionQueryList>.CreateFromQueryResult(entities, take);
        }

        public Task<List<TransactionQueryList>> GetListBetweenDates(DateOnly? startDate, DateOnly? endDate)
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

            return query.Select(TransactionQueryList.Select)
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

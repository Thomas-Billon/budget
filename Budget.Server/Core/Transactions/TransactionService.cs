using Budget.Server.Api.Transactions.Requests;
using Budget.Server.Core.Helpers.Pagination;
using Budget.Server.Data;
using Budget.Server.Data.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Core.Transactions
{
    public class TransactionService
    {
        const int NO_ENTRIES_WRITTEN_TO_DATABASE = 0;

        private readonly ApplicationDbContext _context;
        private readonly PaginationService _paginationService;

        public TransactionService
        (
            ApplicationDbContext context,
            PaginationService paginationService
        )
        {
            _context = context;
            _paginationService = paginationService;
        }

        public async Task<Pagination<TransactionQuery>> GetList(int skip, int take, HashSet<TransactionFilterOption> filters, TransactionSortOption sort)
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

            query = _paginationService.ApplyPaginationToQuery(query, skip, take);

            var items = await query.Select(TransactionQuery.Select)
                .ToListAsync();

            return _paginationService.CreatePagination(items, take);
        }

        public Task<TransactionQuery?> GetById(int id)
        {
            return _context.Transactions.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(TransactionQuery.Select)
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
                return NO_ENTRIES_WRITTEN_TO_DATABASE;
            }

            dbEntity.Type = entity.Type;
            dbEntity.Amount = entity.Amount;
            dbEntity.Title = entity.Title;
            dbEntity.Date = entity.Date;
            dbEntity.PaymentMethod = entity.PaymentMethod;
            dbEntity.Comment = entity.Comment;

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

using Budget.Server.Api.Accounts.Models.Requests;
using Budget.Server.Core.Accounts.Models;
using Budget.Server.Data;
using Budget.Server.Data.Accounts;
using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Core.Accounts
{
    public class AccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService
        (
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        public Task<List<AccountQuery>> GetOptionsForSelectField(string userId)
        {
            return GetAll_AsQueryable(userId).AsNoTracking()
                .Select(AccountQuery.Select)
                .ToListAsync();
        }

        #region CRUD

        public Task<List<AccountQueryList>> GetList(string userId)
        {
            return GetAll_AsQueryable(userId).AsNoTracking()
                .Select(AccountQueryList.Select)
                .ToListAsync();
        }

        public Task<AccountQueryDetails?> GetDetails(int id, string userId)
        {
            return GetById_AsQueryable(id, userId).AsNoTracking()
                .Select(AccountQueryDetails.Select)
                .FirstOrDefaultAsync();
        }

        public Task<int> Create(AccountCreateRequest request, string userId)
        {
            var entity = new Account
            {
                Name = request.Name,
                Bank = request.Bank,
                Currency = request.Currency,
                UserId = userId,
            };

            _context.Accounts.Add(entity);

            return _context.SaveChangesAsync();
        }

        public async Task<int> Update(int id, AccountUpdateRequest request, string userId)
        {
            var entity = await GetById_AsQueryable(id, userId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return 0;
            }

            entity.Name = request.Name;
            entity.Bank = request.Bank;
            entity.Currency = request.Currency;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Patch(int id, AccountPatchRequest request, string userId)
        {
            var entity = await GetById_AsQueryable(id, userId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return 0;
            }

            if (request.Name?.IsSet == true) entity.Name = request.Name.Value ?? string.Empty;
            if (request.Bank?.IsSet == true) entity.Bank = request.Bank.Value;
            if (request.Currency?.IsSet == true) entity.Currency = request.Currency.Value;

            return await _context.SaveChangesAsync();
        }

        public Task<int> Delete(int id, string userId)
        {
            return GetById_AsQueryable(id, userId)
                .ExecuteDeleteAsync();
        }

        #endregion CRUD

        #region Queryable

        private IQueryable<Account> GetAll_AsQueryable(string userId)
        {
            return _context.Accounts
                .Where(x => x.UserId == userId);
        }

        private IQueryable<Account> GetById_AsQueryable(int id, string userId)
        {
            return _context.Accounts
                .Where(x => x.Id == id)
                .Where(x => x.UserId == userId);
        }

        #endregion Queryable
    }
}

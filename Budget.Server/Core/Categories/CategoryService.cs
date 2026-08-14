using Budget.Server.Api.Categories.Models.Requests;
using Budget.Server.Core.Categories.Models;
using Budget.Server.Data;
using Budget.Server.Data.Categories;
using Microsoft.EntityFrameworkCore;

namespace Budget.Server.Core.Categories
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService
        (
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        public Task<List<CategoryQuery>> GetOptionsForSelectField(string userId)
        {
            return GetAll_AsQueryable(userId).AsNoTracking()
                .Select(CategoryQuery.Select)
                .ToListAsync();
        }

        #region CRUD

        public Task<List<CategoryQueryList>> GetList(string userId)
        {
            return GetAll_AsQueryable(userId).AsNoTracking()
                .Select(CategoryQueryList.Select)
                .ToListAsync();
        }

        public Task<CategoryQueryDetails?> GetDetails(int id, string userId)
        {
            return GetById_AsQueryable(id, userId).AsNoTracking()
                .Select(CategoryQueryDetails.Select)
                .FirstOrDefaultAsync();
        }

        public Task<int> Create(CategoryCreateRequest request, string userId)
        {
            var entity = new Category
            {
                Name = request.Name,
                Color = request.Color,
                Icon = request.Icon,
                UserId = userId,
            };

            _context.Categories.Add(entity);

            return _context.SaveChangesAsync();
        }

        public async Task<int> Update(int id, CategoryUpdateRequest request, string userId)
        {
            var entity = await GetById_AsQueryable(id, userId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return 0;
            }

            entity.Name = request.Name;
            entity.Color = request.Color;
            entity.Icon = request.Icon;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Patch(int id, CategoryPatchRequest request, string userId)
        {
            var entity = await GetById_AsQueryable(id, userId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return 0;
            }

            if (request.Name?.IsSet == true) entity.Name = request.Name.Value ?? string.Empty;
            if (request.Color?.IsSet == true) entity.Color = request.Color.Value;
            if (request.Icon?.IsSet == true) entity.Icon = request.Icon.Value;

            return await _context.SaveChangesAsync();
        }

        public Task<int> Delete(int id, string userId)
        {
            return GetById_AsQueryable(id, userId)
                .ExecuteDeleteAsync();
        }

        #endregion CRUD

        #region Queryable

        private IQueryable<Category> GetAll_AsQueryable(string userId)
        {
            return _context.Categories
                .Where(x => x.UserId == userId);
        }

        private IQueryable<Category> GetById_AsQueryable(int id, string userId)
        {
            return _context.Categories
                .Where(x => x.Id == id)
                .Where(x => x.UserId == userId);
        }

        #endregion Queryable
    }
}

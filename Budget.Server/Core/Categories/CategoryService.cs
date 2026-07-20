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

        public Task<List<CategoryQueryFieldOptions>> GetCategoryFieldOptions(string userId)
        {
            return GetCategories_AsQueryable(userId).AsNoTracking()
                .Select(CategoryQueryFieldOptions.Select)
                .ToListAsync();
        }

        public Task<List<CategoryQueryList>> GetCategoryList(string userId)
        {
            return GetCategories_AsQueryable(userId).AsNoTracking()
                .Select(CategoryQueryList.Select)
                .ToListAsync();
        }

        public Task<List<CategoryQueryBalance>> GetCategoryBalance(string userId)
        {
            return GetCategories_AsQueryable(userId).AsNoTracking()
                .Select(CategoryQueryBalance.Select)
                .ToListAsync();
        }

        public Task<CategoryQueryDetails?> GetCategoryDetails(int id, string userId)
        {
            return GetCategoryById_AsQueryable(id, userId).AsNoTracking()
                .Select(CategoryQueryDetails.Select)
                .FirstOrDefaultAsync();
        }

        public Task<int> CreateCategory(CategoryCreateRequest request, string userId)
        {
            var entity = new Category
            {
                Name = request.Name,
                Color = request.Color,
                UserId = userId,
            };

            _context.Categories.Add(entity);

            return _context.SaveChangesAsync();
        }

        public async Task<int> UpdateCategory(int id, CategoryUpdateRequest request, string userId)
        {
            var entity = await GetCategoryById_AsQueryable(id, userId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return 0;
            }

            entity.Name = request.Name;
            entity.Color = request.Color;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> PatchCategory(int id, CategoryPatchRequest request, string userId)
        {
            var entity = await GetCategoryById_AsQueryable(id, userId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return 0;
            }

            if (request.Name?.IsSet == true) entity.Name = request.Name.Value ?? string.Empty;
            if (request.Color?.IsSet == true) entity.Color = request.Color.Value;

            return await _context.SaveChangesAsync();
        }

        public Task<int> DeleteCategory(int id, string userId)
        {
            return GetCategoryById_AsQueryable(id, userId)
                .ExecuteDeleteAsync();
        }

        #region Private

        #region Get data

        private IQueryable<Category> GetCategories_AsQueryable(string userId)
        {
            return _context.Categories
                .Where(x => x.UserId == userId);
        }

        private IQueryable<Category> GetCategoryById_AsQueryable(int id, string userId)
        {
            return _context.Categories
                .Where(x => x.Id == id)
                .Where(x => x.UserId == userId);
        }

        #endregion Get data

        #endregion Private
    }
}

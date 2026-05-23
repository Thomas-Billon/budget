using Budget.Server.Api.Categories.Models.Requests;
using Budget.Server.Core.Enums;
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

        public Task<List<CategoryQueryOptions>> GetCategoryOptions()
        {
            return _context.Categories.AsNoTracking()
                .Select(CategoryQueryOptions.Select)
                .ToListAsync();
        }

        public async Task<List<CategoryQueryHierarchy>> GetCategoryHierarchy()
        {
            var categories = await _context.Categories.AsNoTracking()
                .Select(CategoryQueryHierarchy.Select)
                .ToListAsync();

            return BuildCategoryHierarchyFromList(categories);
        }

        public Task<List<CategoryQueryBalance>> GetCategoryBalance()
        {
            return _context.Categories.AsNoTracking()
                .Select(CategoryQueryBalance.Select)
                .ToListAsync();
        }

        public Task<CategoryQueryDetails?> GetCategoryDetails(int id)
        {
            return _context.Categories.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(CategoryQueryDetails.Select)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateCategory(CategoryCreateRequest request)
        {
            var entity = new Category
            {
                Name = request.Name,
                Color = request.Color,
            };

            // Parent category
            await SetParentCategory(entity, request.ParentCategoryId);

            _context.Categories.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateCategory(int id, CategoryUpdateRequest request)
        {
            var entity = await GetCategoryById(id);
            if (entity == null)
            {
                return 0;
            }

            entity.Name = request.Name;
            entity.Color = request.Color;

            // Parent category
            await SetParentCategory(entity, request.ParentCategoryId);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> PatchCategory(int id, CategoryPatchRequest request)
        {
            var entity = await GetCategoryById(id);
            if (entity == null)
            {
                return 0;
            }

            if (request.Name?.IsSet == true) entity.Name = request.Name.Value ?? string.Empty;
            if (request.Color?.IsSet == true) entity.Color = request.Color.Value;

            // Parent category
            if (request.ParentCategoryId?.IsSet == true)
            {
                await SetParentCategory(entity, request.ParentCategoryId.Value);
            }

            return await _context.SaveChangesAsync();
        }

        public Task<int> DeleteCategory(int id)
        {
            return _context.Categories
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }

        #region Private

        private Task<Category?> GetCategoryById(int id)
        {
            return _context.Categories
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        #region Parent category

        private async Task SetParentCategory(Category entity, int? parentCategoryId)
        {
            if (parentCategoryId == null)
            {
                entity.ParentCategoryId = null;
                return;
            }

            var parentCategory = await _context.Categories
                .Where(x => x.Id == parentCategoryId)
                .FirstOrDefaultAsync();

            if (parentCategory != null)
            {
                entity.ParentCategory = parentCategory;
            }
        }

        #endregion Parent category

        #region Hierarchy

        private List<CategoryQueryHierarchy> BuildCategoryHierarchyFromList(List<CategoryQueryHierarchy> categories)
        {
            var lookup = categories.ToLookup(x => x.ParentCategoryId);

            foreach (var category in categories)
            {
                category.SubCategories = lookup[category.Base.Id].ToList();
            }

            return lookup[null].ToList();
        }

        #endregion Hierarchy

        #endregion Private
    }
}

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

        public Task<List<CategoryQuery_Options>> GetCategoryOptions()
        {
            return _context.Categories.AsNoTracking()
                .Select(CategoryQuery_Options.Select)
                .ToListAsync();
        }

        public async Task<List<CategoryQuery_Hierarchy>> GetCategoryHierarchy()
        {
            var categories = await _context.Categories.AsNoTracking()
                .Select(CategoryQuery_Hierarchy.Select)
                .ToListAsync();

            return BuildCategoryHierarchyFromList(categories);
        }

        public Task<CategoryQuery_Details?> GetCategoryDetails(int id)
        {
            return _context.Categories.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(CategoryQuery_Details.Select)
                .FirstOrDefaultAsync();
        }

        public Task<int> CreateCategory(CategoryCreateParameters parameters)
        {
            var entity = new Category
            {
                Name = parameters.Request.Name,
                Color = parameters.Request.Color,
                ParentCategoryId = parameters.IsParentCategoryValid ? parameters.Request.ParentCategoryId : null,
            };

            _context.Categories.Add(entity);

            return _context.SaveChangesAsync();
        }

        public Task<int> UpdateCategory(int id, CategoryUpdateParameters parameters)
        {
            return _context.Categories
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Name, parameters.Request.Name)
                    .SetProperty(x => x.Color, parameters.Request.Color)
                    .SetProperty(x => x.ParentCategoryId, parameters.IsParentCategoryValid ? parameters.Request.ParentCategoryId : null)
                );
        }

        public async Task<int> PatchCategory(int id, CategoryPatchParameters parameters)
        {
            var entity = await _context.Categories.FindAsync(id);

            if (entity == null)
            {
                return 0;
            }

            if (parameters.Request.Name?.IsSet == true) entity.Name = parameters.Request.Name.Value ?? string.Empty;
            if (parameters.Request.Color?.IsSet == true) entity.Color = parameters.Request.Color.Value;
            if (parameters.Request.ParentCategoryId?.IsSet == true)
            {
                entity.ParentCategoryId = parameters.IsParentCategoryValid ? parameters.Request.ParentCategoryId.Value : null;
            }

            return await _context.SaveChangesAsync();
        }

        public Task<int> DeleteCategory(int id)
        {
            return _context.Categories
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }

        #region Exists

        public Task<bool> DoesCategoryExist(int id)
        {
            return _context.Categories.AsNoTracking()
                .Where(x => x.Id == id)
                .AnyAsync();
        }

        public async Task<Dictionary<int, bool>> DoesCategoriesExist(List<int> ids)
        {
            var result = await _context.Categories.AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, x => true);

            foreach (var id in ids)
            {
                if (!result.ContainsKey(id))
                {
                    result.Add(id, false);
                }
            }

            return result;
        }

        #endregion Exists

        #region Hierarchy

        private List<CategoryQuery_Hierarchy> BuildCategoryHierarchyFromList(List<CategoryQuery_Hierarchy> categories)
        {
            var lookup = categories.ToLookup(x => x.ParentCategoryId);

            foreach (var category in categories)
            {
                category.SubCategories = lookup[category.Base.Id].ToList();
            }

            return lookup[null].ToList();
        }

        #endregion Hierarchy

        #region Colors

        public string GetCategoryColorHex(CategoryColor color)
        {
            return color switch
            {
                CategoryColor.Blue => "#0000FF",
                CategoryColor.Green => "#00FF00",
                CategoryColor.Yellow => "#FFFF00",
                CategoryColor.Orange => "#FFA500",
                CategoryColor.Red => "#FF0000",
                _ => "#AAAAAA"
            };
        }

        #endregion Colors
    }
}

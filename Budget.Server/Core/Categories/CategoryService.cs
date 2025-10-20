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

        public Task<List<CategoryQuery>> GetAll()
        {
            return _context.Categories.AsNoTracking()
                .Select(CategoryQuery.Select)
                .ToListAsync();
        }

        public async Task<List<CategoryQueryTree>> GetTree()
        {
            var categories = await _context.Categories.AsNoTracking()
                .Select(CategoryQueryTree.Select)
                .ToListAsync();

            return BuildCategoriesTreeFromList(categories);
        }

        public Task<CategoryQueryById?> GetById(int id)
        {
            return _context.Categories.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(CategoryQueryById.Select)
                .FirstOrDefaultAsync();
        }

        public Task<int> Create(CategoryCreateRequest request)
        {
            var entity = new Category
            {
                Name = request.Name,
                Color = request.Color,
                ParentCategoryId = request.ParentCategoryId,
            };

            _context.Categories.Add(entity);

            return _context.SaveChangesAsync();
        }

        public Task<int> Update(int id, CategoryUpdateRequest request)
        {
            return _context.Categories
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Name, request.Name)
                    .SetProperty(x => x.Color, request.Color)
                    .SetProperty(x => x.ParentCategoryId, request.ParentCategoryId)
                );
        }

        public async Task<int> Patch(int id, CategoryPatchRequest request)
        {
            var entity = await _context.Categories.FindAsync(id);

            if (entity == null)
            {
                return 0;
            }

            if (request.Name?.IsSet == true) entity.Name = request.Name.Value ?? string.Empty;
            if (request.Color?.IsSet == true) entity.Color = request.Color.Value;
            if (request.ParentCategoryId?.IsSet == true) entity.ParentCategoryId = request.ParentCategoryId.Value;

            return await _context.SaveChangesAsync();
        }

        public Task<int> Delete(int id)
        {
            return _context.Categories
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }

        #region Tree

        private List<CategoryQueryTree> BuildCategoriesTreeFromList(List<CategoryQueryTree> categories)
        {
            var lookup = categories.ToLookup(x => x.ParentCategoryId);

            foreach (var category in categories)
            {
                category.SubCategories = lookup[category.Base.Id].ToList();
            }

            return lookup[null].ToList();
        }

        #endregion Tree

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

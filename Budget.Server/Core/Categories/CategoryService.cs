using Budget.Server.Api.Categories.Requests;
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

        public Task<List<CategoryQueryAll>> GetAll()
        {
            return _context.Categories.AsNoTracking()
                .Where_IsRoot()
                .Select(CategoryQueryAll.Select)
                .ToListAsync();
        }

        public Task<CategoryQueryById?> GetById(int id)
        {
            return _context.Categories.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(CategoryQueryById.Select)
                .FirstOrDefaultAsync();
        }

        public Task<int> Create(CreateCategoryRequest request)
        {
            var entity = new Category
            {
                Name = request.Name,
                Color = request.Color,
            };

            _context.Categories.Add(entity);

            return _context.SaveChangesAsync();
        }

        public Task<int> Update(int id, UpdateCategoryRequest request)
        {
            return _context.Categories
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Name, request.Name)
                    .SetProperty(x => x.Color, request.Color)
                );
        }

        public async Task<int> Patch(int id, PatchCategoryRequest request)
        {
            var entity = await _context.Categories.FindAsync(id);

            if (entity == null)
            {
                return 0;
            }

            if (request.Name?.IsSet == true) entity.Name = request.Name.Value ?? string.Empty;
            if (request.Color?.IsSet == true) entity.Color = request.Color.Value;

            return await _context.SaveChangesAsync();
        }

        public Task<int> Delete(int id)
        {
            return _context.Categories
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }

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

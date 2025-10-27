using Budget.Server.Api.Categories.Models.Requests;
using Budget.Server.Api.Categories.Models.Responses;
using Budget.Server.Core.Categories;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Server.Api.Categories
{
    [ApiController]
	[Route("[controller]")]
	public class CategoryController : ControllerBase
    {
		private readonly CategoryService _categoryService;

        public CategoryController
        (
            CategoryService categoryService
        )
        {
            _categoryService = categoryService;
        }

        [HttpGet("options")]
        public async Task<ActionResult<CategoryOptionsResponse>> GetCategoryOptions()
        {
            var categories = await _categoryService.GetCategoryOptions();

            var response = new CategoryOptionsResponse
            {
                Items = categories.Select(x => new CategoryOptionsItemResponse()
                {
                    Id = x.Base.Id,
                    Name = x.Base.Name,
                    Color = x.Base.Color,
                    ColorHex = _categoryService.GetCategoryColorHex(x.Base.Color),
                }).ToList(),
            };

            return Ok(response);
        }

        [HttpGet("hierarchy")]
        public async Task<ActionResult<CategoryHierarchyResponse>> GetCategoryHierarchy()
        {
            var categories = await _categoryService.GetCategoryHierarchy();

            var response = new CategoryHierarchyResponse
            {
                Items = new(),
            };

            foreach (var category in categories)
            {
                response.Items.Add(ToCategoryHierarchyItemResponse(category));
            }

            return Ok(response);
        }

        [HttpGet("{id:int}")]
		public async Task<ActionResult<CategoryDetailsResponse?>> Details(int id)
        {
            var category = await _categoryService.GetCategoryDetails(id);
            if (category == null)
            {
                return NotFound();
            }

            var response = new CategoryDetailsResponse
            {
                Id = category.Base.Id,
                Name = category.Base.Name,
                Color = category.Base.Color,
                ColorHex = _categoryService.GetCategoryColorHex(category.Base.Color),
                ParentCategoryId = category.ParentCategoryId,
                SubCategories = category.SubCategories
                    .Select(x => new CategoryDetailsBaseResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Color = x.Color,
                        ColorHex = _categoryService.GetCategoryColorHex(x.Color),
                    })
                    .ToList(),
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryCreateRequest request)
        {
            var parameters = new CategoryCreateParameters(request, isParentCategoryValid: true);

            if (request.ParentCategoryId != null)
            {
                parameters.IsParentCategoryValid = await _categoryService.DoesCategoryExist(request.ParentCategoryId.Value);
            }

            var result = await _categoryService.CreateCategory(parameters);
            if (result == 0)
            {
                return BadRequest("Category creation failed.");
            }

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateRequest request)
        {
            var parameters = new CategoryUpdateParameters(request, isParentCategoryValid: true);

            if (request.ParentCategoryId != null)
            {
                parameters.IsParentCategoryValid = await _categoryService.DoesCategoryExist(request.ParentCategoryId.Value);
            }

            var result = await _categoryService.UpdateCategory(id, parameters);
            if (result == 0)
            {
                return BadRequest("Category update failed.");
            }

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchCategory(int id, [FromBody] CategoryPatchRequest request)
        {
            var parameters = new CategoryPatchParameters(request, isParentCategoryValid: true);

            if (request.ParentCategoryId?.IsSet == true && request.ParentCategoryId.Value != null)
            {
                parameters.IsParentCategoryValid = await _categoryService.DoesCategoryExist(request.ParentCategoryId.Value.Value);
            }

            var result = await _categoryService.PatchCategory(id, parameters);
            if (result == 0)
            {
                return BadRequest("Category patch failed.");
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategory(id);
            if (result == 0)
            {
                return BadRequest("Category deletion failed.");
            }

            return Ok();
        }

        #region Hierarchy

        private CategoryHierarchyItemResponse ToCategoryHierarchyItemResponse(CategoryQuery_Hierarchy category)
        {
            var result = new CategoryHierarchyItemResponse
            {
                Id = category.Base.Id,
                Name = category.Base.Name,
                Color = category.Base.Color,
                ColorHex = _categoryService.GetCategoryColorHex(category.Base.Color),
                ParentCategoryId = category.ParentCategoryId,
                SubCategories = new(),
            };

            // Avoids recursion inside select
            foreach (var subCategory in category.SubCategories)
            {
                result.SubCategories.Add(ToCategoryHierarchyItemResponse(subCategory));
            }

            return result;
        }

        #endregion Hierarchy
    }
}

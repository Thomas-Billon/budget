using Budget.Server.Api.Categories.Models.Requests;
using Budget.Server.Api.Categories.Models.Responses;
using Budget.Server.Core.Categories;
using Budget.Server.Core.Categories.Enums;
using Budget.Server.Core.Categories.Models;
using Budget.Server.Core.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Budget.Server.Api.Categories
{
    [Authorize]
    [Route("[controller]")]
    public class CategoryController : ApiControllerBase
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
            var userId = GetUserId();

            var categories = await _categoryService.GetCategoryOptions(userId);

            var response = new CategoryOptionsResponse
            {
                Items = categories.Select(x => new CategoryOptionsItemResponse
                {
                    Id = x.Base.Id,
                    Name = x.Base.Name,
                    Color = x.Base.Color,
                    ColorHex = x.Base.Color.ToHex(),
                }).ToList(),
            };

            return Ok(response);
        }

        [HttpGet("hierarchy")]
        public async Task<ActionResult<CategoryHierarchyResponse>> GetCategoryHierarchy()
        {
            var userId = GetUserId();

            var categories = await _categoryService.GetCategoryHierarchy(userId);

            var response = new CategoryHierarchyResponse
            {
                Items = [],
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
            var userId = GetUserId();

            var category = await _categoryService.GetCategoryDetails(id, userId);
            if (category == null)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Category.NotFound);
            }

            var response = new CategoryDetailsResponse
            {
                Id = category.Base.Id,
                Name = category.Base.Name,
                Color = category.Base.Color,
                ColorHex = category.Base.Color.ToHex(),
                ParentCategoryId = category.ParentCategoryId,
                SubCategories = category.SubCategories
                    .Select(x => new CategoryDetailsBaseResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Color = x.Color,
                        ColorHex = x.Color.ToHex(),
                    })
                    .ToList(),
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateRequest request)
        {
            var userId = GetUserId();

            var result = await _categoryService.CreateCategory(request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.BadRequest, ErrorCodes.Category.CannotCreate);
            }

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateRequest request)
        {
            var userId = GetUserId();

            var result = await _categoryService.UpdateCategory(id, request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Category.CannotUpdate);
            }

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchCategory(int id, [FromBody] CategoryPatchRequest request)
        {
            var userId = GetUserId();

            var result = await _categoryService.PatchCategory(id, request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Category.CannotPatch);
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = GetUserId();

            var result = await _categoryService.DeleteCategory(id, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Category.CannotDelete);
            }

            return Ok();
        }

        #region Private

        #region Hierarchy

        private CategoryHierarchyItemResponse ToCategoryHierarchyItemResponse(CategoryQueryHierarchy category)
        {
            var result = new CategoryHierarchyItemResponse
            {
                Id = category.Base.Id,
                Name = category.Base.Name,
                Color = category.Base.Color,
                ColorHex = category.Base.Color.ToHex(),
                ParentCategoryId = category.ParentCategoryId,
                SubCategories = [],
            };

            // Avoids recursion inside select
            foreach (var subCategory in category.SubCategories)
            {
                result.SubCategories.Add(ToCategoryHierarchyItemResponse(subCategory));
            }

            return result;
        }

        #endregion Hierarchy

        #endregion Private
    }
}

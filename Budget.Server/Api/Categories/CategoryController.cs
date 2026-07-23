using Budget.Server.Api.Categories.Models.Requests;
using Budget.Server.Api.Categories.Models.Responses;
using Budget.Server.Core.Categories;
using Budget.Server.Core.Categories.Enums;
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
        public async Task<ActionResult<CategoryOptionsResponse>> GetOptionsForSelectField()
        {
            var userId = GetUserId();

            var categories = await _categoryService.GetOptionsForSelectField(userId);

            var response = new CategoryOptionsResponse
            {
                Items = categories.Select(x => new CategoryOptionsItemResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Color = x.Color,
                    ColorHex = x.Color.ToHex(),
                }).ToList(),
            };

            return Ok(response);
        }

        #region CRUD

        [HttpGet("list")]
        public async Task<ActionResult<CategoryListResponse>> GetList()
        {
            var userId = GetUserId();

            var categories = await _categoryService.GetList(userId);

            var response = new CategoryListResponse
            {
                Items = categories.Select(x => new CategoryListItemResponse
                {
                    Id = x.Base.Id,
                    Name = x.Base.Name,
                    Color = x.Base.Color,
                    ColorHex = x.Base.Color.ToHex(),
                }).ToList(),
            };

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDetailsResponse?>> GetDetails(int id)
        {
            var userId = GetUserId();

            var category = await _categoryService.GetDetails(id, userId);
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
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateRequest request)
        {
            var userId = GetUserId();

            var result = await _categoryService.Create(request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.BadRequest, ErrorCodes.Category.CannotCreate);
            }

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateRequest request)
        {
            var userId = GetUserId();

            var result = await _categoryService.Update(id, request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Category.CannotUpdate);
            }

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] CategoryPatchRequest request)
        {
            var userId = GetUserId();

            var result = await _categoryService.Patch(id, request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Category.CannotPatch);
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var result = await _categoryService.Delete(id, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Category.CannotDelete);
            }

            return Ok();
        }

        #endregion CRUD
    }
}

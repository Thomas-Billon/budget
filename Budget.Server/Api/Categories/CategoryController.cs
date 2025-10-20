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

        [HttpGet]
        public async Task<ActionResult<CategoryFlatListResponse>> FlatList()
        {
            var categories = await _categoryService.GetAll();

            var response = new CategoryFlatListResponse
            {
                Items = categories.Select(x => new CategoryFlatListItemResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Color = x.Color,
                    ColorHex = _categoryService.GetCategoryColorHex(x.Color),
                }).ToList(),
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("tree")]
        public async Task<ActionResult<CategoryTreeListResponse>> TreeList()
        {
            var categories = await _categoryService.GetTree();

            var response = new CategoryTreeListResponse
            {
                Items = new(),
            };

            foreach (var category in categories)
            {
                response.Items.Add(ToCategoryTreeListItemResponse(category));
            }

            return Ok(response);
        }

		[HttpGet("{id:int}")]
		public async Task<ActionResult<CategoryTreeListItemResponse?>> Details(int id)
        {
            var category = await _categoryService.GetById(id);
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
        public async Task<ActionResult> Create([FromBody] CategoryCreateRequest request)
        {
            var result = await _categoryService.Create(request);
            if (result == 0)
            {
                return BadRequest("Category creation failed.");
            }

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] CategoryUpdateRequest request)
        {
            var result = await _categoryService.Update(id, request);
            if (result == 0)
            {
                return BadRequest("Category update failed.");
            }

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] CategoryPatchRequest request)
        {
            var result = await _categoryService.Patch(id, request);
            if (result == 0)
            {
                return BadRequest("Category patch failed.");
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
		public async Task<ActionResult> Delete(int id)
        {
            var result = await _categoryService.Delete(id);
            if (result == 0)
            {
                return BadRequest("Category deletion failed.");
            }

            return Ok();
        }

        private CategoryTreeListItemResponse ToCategoryTreeListItemResponse(CategoryQueryTree category)
        {
            var result = new CategoryTreeListItemResponse
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
                result.SubCategories.Add(ToCategoryTreeListItemResponse(subCategory));
            }

            return result;
        }
    }
}

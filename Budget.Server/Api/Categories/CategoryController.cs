using Budget.Server.Api.Categories.Requests;
using Budget.Server.Api.Categories.Responses;
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
        public async Task<ActionResult<List<GetAllCategoryResponse>>> GetAll()
        {
            var categories = await _categoryService.GetAll();

            var response = categories.Select(x => new GetAllCategoryResponse
            {
                Id = x.Base.Id,
                Name = x.Base.Name,
                Color = x.Base.Color,
                ColorHex = _categoryService.GetCategoryColorHex(x.Base.Color),
            }).ToList();
            return Ok(response);
        }

		[HttpGet("{id:int}")]
		public async Task<ActionResult<GetAllCategoryResponse?>> GetById(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            var response = new GetByIdCategoryResponse
            {
                Id = category.Base.Id,
                Name = category.Base.Name,
                Color = category.Base.Color,
                ColorHex = _categoryService.GetCategoryColorHex(category.Base.Color),
                SubCategories = category.SubCategories.Select(x => new GetByIdCategoryResponseBase
                {
                    Id = x.Id,
                    Name = x.Name,
                    Color = x.Color,
                    ColorHex = _categoryService.GetCategoryColorHex(x.Color),
                }).ToList()
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var result = await _categoryService.Create(request);
            if (result == 0)
            {
                return BadRequest("Category creation failed.");
            }

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateCategoryRequest request)
        {
            var result = await _categoryService.Update(id, request);
            if (result == 0)
            {
                return BadRequest("Category update failed.");
            }

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] PatchCategoryRequest request)
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
    }
}

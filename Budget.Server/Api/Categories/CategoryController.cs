using Budget.Server.Api.Categories.Requests;
using Budget.Server.Api.Transactions.Responses;
using Budget.Server.Core.Categories;
using Budget.Server.Core.Helpers;
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
        public async Task<ActionResult<List<GetCategoryResponse>>> GetAll()
        {
            var categories = await _categoryService.GetAll();

            var response = categories.Select(ToGetCategoryResponse).ToList();
            return Ok(response);
        }

		[HttpGet("{id:int}")]
		public async Task<ActionResult<GetCategoryResponse?>> GetById(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            var response = ToGetCategoryResponse(category);
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

        private GetCategoryResponse ToGetCategoryResponse(CategoryQuery category)
        {
            return new GetCategoryResponse()
            {
                Id = category.Id,
                Name = category.Name,
                Color = category.Color,
                ColorHex = _categoryService.GetCategoryColorHex(category.Color),
            };
        }
    }
}

using Budget.Server.Api.Categories.Responses;
using Budget.Server.Api.Transactions.Requests;
using Budget.Server.Api.Transactions.Responses;
using Budget.Server.Core.Categories;
using Budget.Server.Core.Helpers;
using Budget.Server.Core.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Server.Api.Transactions
{
    [ApiController]
	[Route("[controller]")]
	public class TransactionController : ControllerBase
    {
		private readonly TransactionService _transactionService;
        private readonly CategoryService _categoryService;

        public TransactionController
        (
            TransactionService transactionService,
            CategoryService categoryService
        )
        {
			_transactionService = transactionService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<GetListTransactionResponse>>> GetList([FromQuery] GetListTransactionRequest request)
        {
            var transactions = await _transactionService.GetList(request.Skip, request.Take, request.Filters, request.Sort);

            var response = new Pagination<GetListTransactionResponse>()
            {
                Page = transactions.Page.Select(x => new GetListTransactionResponse
                {
                    Id = x.Base.Id,
                    Type = x.Base.Type,
                    Amount = x.Base.Amount,
                    Reason = x.Base.Reason,
                    Date = x.Base.Date,
                }).ToList(),
                IsLastPage = transactions.IsLastPage
            };
            return Ok(response);
        }

		[HttpGet("{id:int}")]
		public async Task<ActionResult<GetByIdTransactionResponse?>> GetById(int id)
        {
            var transaction = await _transactionService.GetById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            var response = new GetByIdTransactionResponse
            {
                Id = transaction.Base.Id,
                Type = transaction.Base.Type,
                Amount = transaction.Base.Amount,
                Reason = transaction.Base.Reason,
                Date = transaction.Base.Date,
                PaymentMethod = transaction.Base.PaymentMethod,
                Comment = transaction.Base.Comment,
                Categories = transaction.Categories.Select(x => new GetAllCategoryResponse
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
        public async Task<ActionResult> Create([FromBody] CreateTransactionRequest request)
        {
            var result = await _transactionService.Create(request);
            if (result == 0)
            {
                return BadRequest("Transaction creation failed.");
            }

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateTransactionRequest request)
        {
            var result = await _transactionService.Update(id, request);
            if (result == 0)
            {
                return BadRequest("Transaction update failed.");
            }

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] PatchTransactionRequest request)
        {
            var result = await _transactionService.Patch(id, request);
            if (result == 0)
            {
                return BadRequest("Transaction patch failed.");
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
		public async Task<ActionResult> Delete(int id)
        {
            var result = await _transactionService.Delete(id);
            if (result == 0)
            {
                return BadRequest("Transaction deletion failed.");
            }

            return Ok();
        }
    }
}

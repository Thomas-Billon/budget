using Budget.Server.Api.Categories.Models.Responses;
using Budget.Server.Api.Transactions.Models.Requests;
using Budget.Server.Api.Transactions.Models.Responses;
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
        public async Task<ActionResult<TransactionHistoryResponse>> GetTransactionHistory([FromQuery] TransactionHistoryRequest request)
        {
            var parameters = new TransactionHistoryParameters(request, isPaginationEnabled: true);

            var transactions = await _transactionService.GetTransactionHistory(parameters);
            var paginatedTransactions = transactions.ToPagination(request.Take);

            var response = new TransactionHistoryResponse()
            {
                Page = paginatedTransactions.Page
                    .Select(x => new TransactionHistoryItemResponse
                    {
                        Id = x.Base.Id,
                        Type = x.Base.Type,
                        Amount = x.Base.Amount,
                        Reason = x.Base.Reason,
                        Date = x.Base.Date,
                    })
                    .ToList(),
                IsLastPage = paginatedTransactions.IsLastPage,
            };
            return Ok(response);
        }

		[HttpGet("{id:int}")]
		public async Task<ActionResult<TransactionDetailsResponse?>> Details(int id)
        {
            var transaction = await _transactionService.GetById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            var response = new TransactionDetailsResponse
            {
                Id = transaction.Base.Id,
                Type = transaction.Base.Type,
                Amount = transaction.Base.Amount,
                Reason = transaction.Base.Reason,
                Date = transaction.Base.Date,
                PaymentMethod = transaction.Base.PaymentMethod,
                Comment = transaction.Base.Comment,
                Categories = transaction.Categories
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
        public async Task<ActionResult> Create([FromBody] TransactionCreateRequest request)
        {
            var result = await _transactionService.Create(request);
            if (result == 0)
            {
                return BadRequest("Transaction creation failed.");
            }

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] TransactionUpdateRequest request)
        {
            var result = await _transactionService.Update(id, request);
            if (result == 0)
            {
                return BadRequest("Transaction update failed.");
            }

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] TransactionPatchRequest request)
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

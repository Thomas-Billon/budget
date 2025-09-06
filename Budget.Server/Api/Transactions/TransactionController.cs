using Budget.Server.Api.Transactions.Requests;
using Budget.Server.Api.Transactions.Responses;
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

        public TransactionController
        (
            TransactionService transactionService
        )
        {
			_transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<GetTransactionResponse>>> GetList([FromQuery] GetListTransactionRequest request)
        {
            var transactions = await _transactionService.GetList(request.Skip, request.Take, request.Filters, request.Sort);

            var response = new Pagination<GetTransactionResponse>()
            {
                Page = transactions.Page.Select(ToGetTransactionResponse).ToList(),
                IsLastPage = transactions.IsLastPage
            };
            return Ok(response);
        }

		[HttpGet("{id:int}")]
		public async Task<ActionResult<GetTransactionResponse?>> GetById(int id)
        {
            var transaction = await _transactionService.GetById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            var response = ToGetTransactionResponse(transaction);
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

        private GetTransactionResponse ToGetTransactionResponse(TransactionQuery transaction)
        {
            return new GetTransactionResponse()
            {
                Id = transaction.Id,
                Type = transaction.Type,
                Amount = transaction.Amount,
                Reason = transaction.Reason,
                Date = transaction.Date,
                PaymentMethod = transaction.PaymentMethod,
                Comment = transaction.Comment
            };
        }
    }
}

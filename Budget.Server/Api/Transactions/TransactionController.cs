using Budget.Server.Api.Transactions.Requests;
using Budget.Server.Api.Transactions.Responses;
using Budget.Server.Core.Helpers.Pagination;
using Budget.Server.Core.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Server.Api.Transactions
{
    [ApiController]
	[Route("[controller]")]
	public class TransactionController : ControllerBase
    {
		private readonly TransactionService _transactionService;
        private readonly TransactionMapper _transactionMapper;

        public TransactionController(
            TransactionService transactionService,
            TransactionMapper transactionMapper
        ) {
			_transactionService = transactionService;
            _transactionMapper = transactionMapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<GetTransactionResponse>>> GetList([FromQuery] GetListTransactionRequest request)
        {
            var transactions = await _transactionService.GetList(request.Skip, request.Take, request.Filters, request.Sort);

            var response = _transactionMapper.ToGetListResponse(transactions);
            return Ok(response);
        }

		[HttpGet("{id:int}")]
		public async Task<ActionResult<GetTransactionResponse?>> GetById(int id)
        {
            var transaction = await _transactionService.GetById(id);

            var response = _transactionMapper.ToGetResponse(transaction);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateTransactionRequest request)
        {
            var transaction = _transactionMapper.ToTransaction(request);

            var result = await _transactionService.Create(transaction);
            if (!IsDatabaseOperationResultValid(result))
            {
                return BadRequest("Transaction creation failed.");
            }

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateTransactionRequest request)
        {
            var transaction = _transactionMapper.ToTransaction(request);

            var result = await _transactionService.Update(id, transaction);
            if (!IsDatabaseOperationResultValid(result))
            {
                return BadRequest("Transaction update failed.");
            }

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> UpdatePartial(int id, [FromBody] UpdateTransactionRequest request)
        {
            var transaction = _transactionMapper.ToTransaction(request);

            var result = await _transactionService.UpdatePartial(id, transaction);
            if (!IsDatabaseOperationResultValid(result))
            {
                return BadRequest("Transaction update failed.");
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
		public async Task<ActionResult> Delete(int id)
        {
            var result = await _transactionService.Delete(id);
            if (!IsDatabaseOperationResultValid(result))
            {
                return BadRequest("Transaction deletion failed.");
            }

            return Ok();
        }

        private bool IsDatabaseOperationResultValid(int result)
        {
            return result > 0;
        }
    }
}

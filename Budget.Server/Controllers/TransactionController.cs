using Budget.Server.Dtos;
using Budget.Server.Enums;
using Budget.Server.Mappers;
using Budget.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Server.Controllers
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
		public async Task<ActionResult<TransactionDTO.GetAll.Response>> GetAll()
        {
            var transaction = await _transactionService.GetAll();

            var response = _transactionMapper.ToGetAllResponse(transaction);
            return Ok(response);
        }

		[HttpGet("{id:int}")]
		public async Task<ActionResult<TransactionDTO.Get.Response?>> GetById(int id)
        {
            var transaction = await _transactionService.GetById(id);

            var response = _transactionMapper.ToGetResponse(transaction);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDTO.Create.Response>> Create(TransactionDTO.Create.Request request)
        {
            var transaction = _transactionMapper.ToTransaction(request);

            var result = await _transactionService.Create(transaction);
            if (!IsDatabaseOperationResultValid(result))
            {
                return BadRequest("Transaction creation failed.");
            }

            var response = _transactionMapper.ToCreateResponse(transaction);
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, TransactionDTO.Update.Request request)
        {
            var transaction = _transactionMapper.ToTransaction(request);

            var result = await _transactionService.Update(id, transaction);
            if (!IsDatabaseOperationResultValid(result))
            {
                return BadRequest("Transaction update failed.");
            }

            return Ok();
        }

        [HttpPatch("{id:int}/type")]
        public async Task<ActionResult> UpdateType(int id, TransactionType type)
        {
            var result = await _transactionService.UpdateType(id, type);
            if (!IsDatabaseOperationResultValid(result))
            {
                return BadRequest("Transaction update failed.");
            }

            return Ok();
        }

        [HttpPatch("{id:int}/amount")]
        public async Task<ActionResult> UpdateAmount(int id, double amount)
        {
            var result = await _transactionService.UpdateAmount(id, amount);
            if (!IsDatabaseOperationResultValid(result))
            {
                return BadRequest("Transaction update failed.");
            }

            return Ok();
        }

        [HttpPatch("{id:int}/title")]
        public async Task<ActionResult> UpdateTitle(int id, string title)
        {
            var result = await _transactionService.UpdateTitle(id, title);
            if (!IsDatabaseOperationResultValid(result))
            {
                return BadRequest("Transaction update failed.");
            }

            return Ok();
        }

        [HttpPatch("{id:int}/date")]
        public async Task<ActionResult> UpdateDate(int id, DateOnly? date)
        {
            var result = await _transactionService.UpdateDate(id, date);
            if (!IsDatabaseOperationResultValid(result))
            {
                return BadRequest("Transaction update failed.");
            }

            return Ok();
        }

        [HttpPatch("{id:int}/paymentMethod")]
        public async Task<ActionResult> UpdatePaymentMethod(int id, PaymentMethod paymentMethod)
        {
            var result = await _transactionService.UpdatePaymentMethod(id, paymentMethod);
            if (!IsDatabaseOperationResultValid(result))
            {
                return BadRequest("Transaction update failed.");
            }

            return Ok();
        }

        [HttpPatch("{id:int}/comment")]
        public async Task<ActionResult> UpdateComment(int id, string comment)
        {
            var result = await _transactionService.UpdateComment(id, comment);
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

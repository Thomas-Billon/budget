using Budget.Server.Dtos;
using Budget.Server.Mappers;
using Budget.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Server.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class TransactionController : CustomControllerBase
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
		public Task<ActionResult<TransactionDTO.GetAll.Response>> GetAll()
        {
            return HandleRequest(
                async () => await _transactionService.GetAll(),
                _transactionMapper.ToGetAllResponse
            );
		}

		[HttpGet("{id:int}")]
		public Task<ActionResult<TransactionDTO.Get.Response>> GetById(int id)
		{
            return HandleRequest(
                async () => await _transactionService.GetById(id),
                _transactionMapper.ToGetResponse
            );
		}

        [HttpPost]
        public Task<ActionResult<TransactionDTO.Create.Response>> Create(TransactionDTO.Create.Command command)
        {
            return HandleRequest(
                command,
                _transactionMapper.ToEntity,
                async entity => await _transactionService.Create(entity),
                _transactionMapper.ToCreateResponse
            );
        }

        [HttpPut("{id:int}")]
        public Task<ActionResult<TransactionDTO.Update.Response>> Update(int id, TransactionDTO.Update.Command command)
        {
            return HandleRequest(
                command,
                _transactionMapper.ToEntity,
                async entity => await _transactionService.Update(id, entity),
                _transactionMapper.ToUpdateResponse
            );
        }

        [HttpPut("{id:int}/amount")]
        public Task<ActionResult<TransactionDTO.UpdateAmount.Response>> UpdateAmount(int id, TransactionDTO.UpdateAmount.Command command)
        {
            return HandleRequest(
                command,
                _transactionMapper.ToAmount,
                async amount => await _transactionService.UpdateAmount(id, amount),
                _transactionMapper.ToUpdateAmountResponse
            );
        }

        [HttpPut("{id:int}/date")]
        public Task<ActionResult<TransactionDTO.UpdateDate.Response>> UpdateDate(int id, TransactionDTO.UpdateDate.Command command)
        {
            return HandleRequest(
                command,
                _transactionMapper.ToDate,
                async date => await _transactionService.UpdateDate(id, date),
                _transactionMapper.ToUpdateDateResponse
            );
        }

        [HttpPut("{id:int}/paymentMethod")]
        public Task<ActionResult<TransactionDTO.UpdatePaymentMethod.Response>> UpdatePaymentMethod(int id, TransactionDTO.UpdatePaymentMethod.Command command)
        {
            return HandleRequest(
                command,
                _transactionMapper.ToPaymentMethod,
                async paymentMethod => await _transactionService.UpdatePaymentMethod(id, paymentMethod),
                _transactionMapper.ToUpdatePaymentMethodResponse
            );
        }

        [HttpPut("{id:int}/comment")]
        public Task<ActionResult<TransactionDTO.UpdateComment.Response>> UpdateComment(int id, TransactionDTO.UpdateComment.Command command)
        {
            return HandleRequest(
                command,
                _transactionMapper.ToComment,
                async comment => await _transactionService.UpdateComment(id, comment),
                _transactionMapper.ToUpdateCommentResponse
            );
        }

        [HttpDelete("{id:int}")]
		public Task<ActionResult<ResponseBase>> Delete(int id)
        {
            return HandleRequest(
                async () => await _transactionService.Delete(id)
            );
        }
    }
}

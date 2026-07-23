using Budget.Server.Api.Transactions.Models.Requests;
using Budget.Server.Api.Transactions.Models.Responses;
using Budget.Server.Core.Categories.Enums;
using Budget.Server.Core.Errors;
using Budget.Server.Core.Transactions;
using Budget.Server.Core.Transactions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Budget.Server.Api.Transactions
{
    [Authorize]
    [Route("[controller]")]
    public class TransactionController : ApiControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController
        (
            TransactionService transactionService
        )
        {
            _transactionService = transactionService;
        }

        [HttpGet("history")]
        public async Task<ActionResult<TransactionHistoryResponse>> GetHistory([FromQuery] TransactionHistoryRequest request)
        {
            var userId = GetUserId();
            
            var parameters = TransactionQueryParametersMapper.FromHistoryRequest(request, isPaginationEnabled: true);

            var transactions = await _transactionService.GetHistory(parameters, userId);

            var response = new TransactionHistoryResponse()
            {
                Page = transactions.Page
                    .Select(x => new TransactionHistoryItemResponse
                    {
                        Id = x.Base.Id,
                        Type = x.Base.Type,
                        Amount = x.Base.Amount,
                        Reason = x.Base.Reason,
                        Date = x.Base.Date,
                        Categories = x.Categories
                            .Select(x => new TransactionHistoryCategoryItemResponse
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Color = x.Color,
                                ColorHex = x.Color.ToHex(),
                            })
                            .ToList(),
                    })
                    .ToList(),
                IsLastPage = transactions.IsLastPage,
            };

            return Ok(response);
        }

        #region CRUD 

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TransactionDetailsResponse?>> GetDetails(int id)
        {
            var userId = GetUserId();
            
            var transaction = await _transactionService.GetDetails(id, userId);
            if (transaction == null)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Transaction.NotFound);
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
                    .Select(x => new TransactionDetailsCategoryItemResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Color = x.Color,
                        ColorHex = x.Color.ToHex(),
                    })
                    .ToList(),
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransactionCreateRequest request)
        {
            var userId = GetUserId();

            var result = await _transactionService.Create(request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.BadRequest, ErrorCodes.Transaction.CannotCreate);
            }

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TransactionUpdateRequest request)
        {
            var userId = GetUserId();

            var result = await _transactionService.Update(id, request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Transaction.CannotUpdate);
            }

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] TransactionPatchRequest request)
        {
            var userId = GetUserId();

            var result = await _transactionService.Patch(id, request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Transaction.CannotPatch);
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var result = await _transactionService.Delete(id, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Transaction.CannotDelete);
            }

            return Ok();
        }

        #endregion CRUD
    }
}

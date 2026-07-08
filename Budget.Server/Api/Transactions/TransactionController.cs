using Budget.Server.Api.Transactions.Models.Requests;
using Budget.Server.Api.Transactions.Models.Responses;
using Budget.Server.Core.Categories.Enums;
using Budget.Server.Core.Errors;
using Budget.Server.Core.Shared;
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
        public async Task<ActionResult<TransactionHistoryResponse>> GetTransactionHistory([FromQuery] TransactionHistoryRequest request)
        {
            var userId = GetUserId();
            
            var parameters = TransactionQueryParametersMapper.FromHistoryRequest(request, isPaginationEnabled: true);

            var transactions = await _transactionService.GetTransactionHistory(parameters, userId);
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
                IsLastPage = paginatedTransactions.IsLastPage,
            };

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TransactionDetailsResponse?>> GetTransactionDetails(int id)
        {
            var userId = GetUserId();
            
            var transaction = await _transactionService.GetTransactionDetails(id, userId);
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
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreateRequest request)
        {
            var userId = GetUserId();

            var result = await _transactionService.CreateTransaction(request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.BadRequest, ErrorCodes.Transaction.CannotCreate);
            }

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionUpdateRequest request)
        {
            var userId = GetUserId();

            var result = await _transactionService.UpdateTransaction(id, request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Transaction.CannotUpdate);
            }

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchTransaction(int id, [FromBody] TransactionPatchRequest request)
        {
            var userId = GetUserId();

            var result = await _transactionService.PatchTransaction(id, request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Transaction.CannotPatch);
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var userId = GetUserId();

            var result = await _transactionService.DeleteTransaction(id, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Transaction.CannotDelete);
            }

            return Ok();
        }
    }
}

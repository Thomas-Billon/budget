using Budget.Server.Api.Balances.Models.Requests;
using Budget.Server.Api.Balances.Models.Responses;
using Budget.Server.Core.Balances;
using Budget.Server.Core.Balances.Models;
using Budget.Server.Core.Categories;
using Budget.Server.Core.Categories.Enums;
using Budget.Server.Core.Categories.Models;
using Budget.Server.Core.Transactions;
using Budget.Server.Core.Transactions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Server.Api.Balances
{
    [Authorize]
    [Route("[controller]")]
    public class BalanceController : ApiControllerBase
    {
        private readonly BalanceService _balanceService;
        private readonly TransactionService _transactionService;

        public BalanceController
        (
            BalanceService balanceService,
            TransactionService transactionService
        )
        {
            _balanceService = balanceService;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<BalanceReportResponse>> Report([FromQuery] BalanceReportRequest request)
        {
            var userId = GetUserId();

            var parameters = TransactionQueryParametersMapper.FromBalanceRequest(request);

            var transactions = await _transactionService.GetStats(parameters, userId);
            var report = _balanceService.CalculateReport(transactions);

            var response = new BalanceReportResponse()
            {
                TotalIncome = report.TotalIncome,
                TotalExpense = report.TotalExpense,
                NetBalance = report.NetBalance,
                MostLucrativeTransactions = report.MostLucrativeTransactions
                    .Select(ToTransactionItemResponse)
                    .ToList(),
                MostExpensiveTransactions = report.MostExpensiveTransactions
                    .Select(ToTransactionItemResponse)
                    .ToList(),
                IncomeTransactionsByCategory = report.IncomeTransactionsByCategory
                    .Select(ToTransactionsByCategoryItemResponse)
                    .ToList(),
                ExpenseTransactionsByCategory = report.ExpenseTransactionsByCategory
                    .Select(ToTransactionsByCategoryItemResponse)
                    .ToList(),
            };

            return Ok(response);
        }

        #region Report

        private BalanceReportTransactionItemResponse ToTransactionItemResponse(TransactionQueryStats transaction)
        {
            return new BalanceReportTransactionItemResponse
            {
                Id = transaction.Base.Id,
                Type = transaction.Base.Type,
                Amount = transaction.Base.Amount,
                Reason = transaction.Base.Reason,
                Date = transaction.Base.Date,
            };
        }

        private BalanceReportCategoryItemResponse? ToCategoryItemResponse(CategoryQuery? category)
        {
            if (category == null)
            {
                return null;
            }

            return new BalanceReportCategoryItemResponse
            {
                Id = category.Id,
                Name = category.Name,
                Color = category.Color,
                ColorHex = category.Color.ToHex(),
            };
        }

        private BalanceReportTransactionsByCategoryItemResponse ToTransactionsByCategoryItemResponse(BalanceReportTransactionsByCategoryData transactionsByCategory)
        {
            return new BalanceReportTransactionsByCategoryItemResponse
            {
                Category = ToCategoryItemResponse(transactionsByCategory.Category),
                CategoryShare = transactionsByCategory.CategoryShare,
                Transactions = transactionsByCategory.Transactions.Select(ToTransactionItemResponse).ToList(),
            };
        }

        #endregion Report
    }
}

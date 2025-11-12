using Budget.Server.Api.Balances.Models.Requests;
using Budget.Server.Api.Balances.Models.Responses;
using Budget.Server.Core.Balances;
using Budget.Server.Core.Categories;
using Budget.Server.Core.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Server.Api.Balances
{
    [ApiController]
	[Route("[controller]")]
	public class BalanceController : ControllerBase
    {
        private readonly BalanceService _balanceService;
        private readonly TransactionService _transactionService;
        private readonly CategoryService _categoryService;

        public BalanceController
        (
            BalanceService balanceService,
            TransactionService transactionService,
            CategoryService categoryService
        )
        {
            _balanceService = balanceService;
            _transactionService = transactionService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<BalanceReportResponse>> Report([FromQuery] BalanceReportRequest request)
        {
            var options = new TransactionQueryableOptions(request);

            var transactions = await _transactionService.GetTransactionBalance(options);
            var categories = await _categoryService.GetCategoryBalance();
            var balanceReport = _balanceService.CalculateBalanceReport(transactions);

            var response = new BalanceReportResponse()
            {
                TotalIncome = balanceReport.TotalIncome,
                TotalExpense = balanceReport.TotalExpense,
                NetBalance = balanceReport.NetBalance,
                MostLucrativeTransactions = balanceReport.MostLucrativeTransactions
                    .Select(ToTransactionItemResponse)
                    .ToList(),
                MostExpensiveTransactions = balanceReport.MostExpensiveTransactions
                    .Select(ToTransactionItemResponse)
                    .ToList(),
                Categories = categories
                    .Select(ToCategoryItemResponse)
                    .ToList(),
                IncomeTransactionsByCategory = balanceReport.IncomeTransactionsByCategory
                    .Select(ToTransactionsByCategoryItemResponse)
                    .ToList(),
                ExpenseTransactionsByCategory = balanceReport.ExpenseTransactionsByCategory
                    .Select(ToTransactionsByCategoryItemResponse)
                    .ToList(),
            };
            return Ok(response);
        }

        #region Private

        #region Report

        private BalanceReportTransactionItemResponse ToTransactionItemResponse(TransactionQuery_Balance transaction)
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

        private BalanceReportCategoryItemResponse ToCategoryItemResponse(CategoryQuery_Balance category)
        {
            return new BalanceReportCategoryItemResponse
            {
                Id = category.Base.Id,
                Name = category.Base.Name,
                Color = category.Base.Color,
                ColorHex = _categoryService.GetCategoryColorHex(category.Base.Color),
            };
        }

        private BalanceReportTransactionsByCategoryItemResponse ToTransactionsByCategoryItemResponse(BalanceReportTransactionsByCategoryData transactionsByCategory)
        {
            return new BalanceReportTransactionsByCategoryItemResponse
            {
                CategoryId = transactionsByCategory.CategoryId,
                CategoryShare = transactionsByCategory.CategoryShare,
                Transactions = transactionsByCategory.Transactions.Select(ToTransactionItemResponse).ToList(),
            };
        }

        #endregion Report

        #endregion Private
    }
}

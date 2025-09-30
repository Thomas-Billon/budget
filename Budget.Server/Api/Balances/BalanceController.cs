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
        public async Task<ActionResult<BalanceDetailsResponse>> Details([FromQuery] BalanceDetailsRequest request)
        {
            var transactions = await _transactionService.GetListBetweenDates(request.startDate, request.endDate);
            var balance = _balanceService.CalculateBalanceData(transactions);

            var response = new BalanceDetailsResponse()
            {
                TotalIncome = balance.TotalIncome,
                TotalExpense = balance.TotalExpense,
                NetBalance = balance.NetBalance,
            };
            return Ok(response);
        }
    }
}

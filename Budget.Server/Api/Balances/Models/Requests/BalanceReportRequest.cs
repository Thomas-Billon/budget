using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Api.Balances.Models.Requests
{
    public class BalanceReportRequest
    {
        [Required]
        public DateOnly startDate { get; init; }

        [Required]
        public DateOnly endDate { get; init; }
    }
}

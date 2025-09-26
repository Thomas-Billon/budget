using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Api.Balances.Models.Requests
{
    public class BalanceDetailsRequest
    {
        [Required]
        public DateOnly DateStart { get; init; }

        [Required]
        public DateOnly DateEnd { get; init; }
    }
}

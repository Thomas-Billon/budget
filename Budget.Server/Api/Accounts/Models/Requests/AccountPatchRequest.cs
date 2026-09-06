using Budget.Server.Core.Accounts.Enums;
using Budget.Server.Core.Shared;

namespace Budget.Server.Api.Accounts.Models.Requests
{
    public class AccountPatchRequest
    {
        public Optional<string>? Name { get; init; }

        public Optional<Bank>? Bank { get; init; }

        public Optional<Currency>? Currency { get; init; }
    }
}

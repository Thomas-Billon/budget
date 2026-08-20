using Budget.Server.Api.Accounts.Models.Requests;
using Budget.Server.Api.Accounts.Models.Responses;
using Budget.Server.Core.Accounts;
using Budget.Server.Core.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Budget.Server.Api.Accounts
{
    [Authorize]
    [Route("[controller]")]
    public class AccountController : ApiControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController
        (
            AccountService accountService
        )
        {
            _accountService = accountService;
        }

        [HttpGet("options")]
        public async Task<ActionResult<AccountOptionsResponse>> GetOptionsForSelectField()
        {
            var userId = GetUserId();

            var accounts = await _accountService.GetOptionsForSelectField(userId);

            var response = new AccountOptionsResponse
            {
                Items = accounts.Select(x => new AccountOptionsItemResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList(),
            };

            return Ok(response);
        }

        #region CRUD

        [HttpGet("list")]
        public async Task<ActionResult<AccountListResponse>> GetList()
        {
            var userId = GetUserId();

            var accounts = await _accountService.GetList(userId);

            var response = new AccountListResponse
            {
                Items = accounts.Select(x => new AccountListItemResponse
                {
                    Id = x.Base.Id,
                    Name = x.Base.Name,
                    Bank = x.Base.Bank,
                    Currency = x.Base.Currency,
                    TransactionCount = x.TransactionCount,
                }).ToList(),
            };

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AccountDetailsResponse?>> GetDetails(int id)
        {
            var userId = GetUserId();

            var account = await _accountService.GetDetails(id, userId);
            if (account == null)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Account.NotFound);
            }

            var response = new AccountDetailsResponse
            {
                Id = account.Base.Id,
                Name = account.Base.Name,
                Bank = account.Base.Bank,
                Currency = account.Base.Currency,
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountCreateRequest request)
        {
            var userId = GetUserId();

            var result = await _accountService.Create(request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.BadRequest, ErrorCodes.Account.CannotCreate);
            }

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountUpdateRequest request)
        {
            var userId = GetUserId();

            var result = await _accountService.Update(id, request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Account.CannotUpdate);
            }

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] AccountPatchRequest request)
        {
            var userId = GetUserId();

            var result = await _accountService.Patch(id, request, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Account.CannotPatch);
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var result = await _accountService.Delete(id, userId);
            if (result == 0)
            {
                return Failure(HttpStatusCode.NotFound, ErrorCodes.Account.CannotDelete);
            }

            return Ok();
        }

        #endregion CRUD
    }
}

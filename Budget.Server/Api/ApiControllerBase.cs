using Budget.Server.Core.Auth.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Budget.Server.Api
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected string GetUserId()
        {
            // INFO: MapInboundClaims is set to false so we can use claim names instead of .NET-style URIs.
            var userId = User.FindFirst(ClaimNames.Id)?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException();
            }

            return userId;
        }

        protected ActionResult Failure(HttpStatusCode status, string error) =>
            StatusCode((int)status, new { error });
    }
}

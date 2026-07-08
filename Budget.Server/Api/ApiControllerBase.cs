using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Budget.Server.Api
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ActionResult Failure(HttpStatusCode status, string error) =>
            StatusCode((int)status, new { error });
    }
}

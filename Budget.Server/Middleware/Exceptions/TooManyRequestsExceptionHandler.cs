using Budget.Server.Core.Errors;
using Microsoft.AspNetCore.Diagnostics;

namespace Budget.Server.Middleware.Exceptions
{
    public class TooManyRequestsExceptionHandler : GlobalExceptionHandler, IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not TooManyRequestsException tooManyRequestsException)
            {
                return false;
            }

            if (tooManyRequestsException.RetryAfter != null)
            {
                httpContext.Response.Headers.RetryAfter = ((int)tooManyRequestsException.RetryAfter.Value.TotalSeconds).ToString();
            }

            await HandleAsync(StatusCodes.Status429TooManyRequests, ErrorCodes.Server.TooManyRequests, httpContext, cancellationToken);
            return true;
        }
    }
}

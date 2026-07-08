using Budget.Server.Core.Errors;
using Microsoft.AspNetCore.Diagnostics;

namespace Budget.Server.Middleware.Exceptions
{
    public class UnauthorizedExceptionHandler : GlobalExceptionHandler, IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not UnauthorizedAccessException)
            {
                return false;
            }

            await HandleAsync(StatusCodes.Status401Unauthorized, ErrorCodes.Server.Unauthorized, httpContext, cancellationToken);
            return true;
        }
    }
}

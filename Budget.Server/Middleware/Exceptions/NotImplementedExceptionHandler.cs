using Budget.Server.Core.Errors;
using Microsoft.AspNetCore.Diagnostics;

namespace Budget.Server.Middleware.Exceptions
{
    public class NotImplementedExceptionHandler : GlobalExceptionHandler, IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not NotImplementedException)
            {
                return false;
            }

            await HandleAsync(StatusCodes.Status501NotImplemented, ErrorCodes.Server.NotImplemented, httpContext, cancellationToken);
            return true;
        }
    }
}

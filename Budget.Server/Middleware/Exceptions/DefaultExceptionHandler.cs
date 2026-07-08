using Budget.Server.Core.Errors;
using Microsoft.AspNetCore.Diagnostics;

namespace Budget.Server.Middleware.Exceptions
{
    public class DefaultExceptionHandler : GlobalExceptionHandler, IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            await HandleAsync(StatusCodes.Status500InternalServerError, ErrorCodes.Server.Internal, httpContext, cancellationToken);
            return true;
        }
    }
}

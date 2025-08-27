using Microsoft.AspNetCore.Diagnostics;

namespace Budget.Server.Middleware.Error
{
    public class DefaultExceptionHandler : GlobalExceptionHandler, IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            return HandleAsync(StatusCodes.Status500InternalServerError, httpContext, exception, cancellationToken);
        }
    }
}

using Microsoft.AspNetCore.Diagnostics;

namespace Budget.Server.Middlewares.Error
{
    public class DefaultGlobalExceptionHandler : GlobalExceptionHandler, IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            return HandleAsync(StatusCodes.Status500InternalServerError, httpContext, exception, cancellationToken);
        }
    }
}

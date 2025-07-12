using Microsoft.AspNetCore.Diagnostics;

namespace Budget.Server.Middlewares.Error
{
    public class NotImplementedExceptionHandler : GlobalExceptionHandler, IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not NotImplementedException)
            {
                return false;
            }

            return await HandleAsync(StatusCodes.Status501NotImplemented, httpContext, exception, cancellationToken);
        }
    }
}

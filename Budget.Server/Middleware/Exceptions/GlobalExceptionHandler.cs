namespace Budget.Server.Middleware.Exceptions
{
    public class GlobalExceptionHandler
    {
        public Task HandleAsync(int statusCode, string error, HttpContext httpContext, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = statusCode;
            return httpContext.Response.WriteAsJsonAsync(new { error }, cancellationToken);
        }
    }
}

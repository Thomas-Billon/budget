namespace Budget.Server.Middleware.Error
{
    public class ExceptionResponse
    {
        public int StatusCode { get; set; } = StatusCodes.Status200OK;
        public string Message { get; set; } = string.Empty;
    }

    public class GlobalExceptionHandler
    {
        protected async ValueTask<bool> HandleAsync(int statusCode, HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var response = new ExceptionResponse
            {
                StatusCode = statusCode,
                Message = exception.Message
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}

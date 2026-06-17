namespace Savemate.Web.Middleware
{
    public class ExceptionalLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionalLoggingMiddleware> _logger;

        public ExceptionalLoggingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionalLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception on {Method} {Path} by user {UserId}",
                    context.Request.Method,
                    context.Request.Path,
                    context.User?.Identity?.Name ?? "anonymous");

                throw; // re-throw so ASP.NET Core's error handling still works
            }
        }
    }
}

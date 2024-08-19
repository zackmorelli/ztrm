namespace ztrm.Middleware
{
    public class ErrorLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorLogger> _logger;

        public ErrorLogger(RequestDelegate next, ILogger<ErrorLogger> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Call the next middleware in the pipeline
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw; // Re-throw the exception to be handled by other middleware or error handling pages
            }
        }

        private void LogError(Exception ex)
        {
            var methodBase = ex.TargetSite;
            var methodName = methodBase?.Name ?? "Unknown Method";

            var declaringType = methodBase?.DeclaringType;
            var typeName = declaringType != null ? declaringType.FullName : "Unknown Type";

            var errorLocation = $"{typeName}.{methodName}";

            _logger.LogError(ex, $"An unhandled exception occurred in {errorLocation}: {ex.Message}");
        }
    }
}
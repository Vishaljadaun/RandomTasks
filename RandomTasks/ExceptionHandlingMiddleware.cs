using System.Text.Json;

namespace RandomTasks
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Serilog will automatically format this
                _logger.LogError(ex, "Unhandled exception occurred while processing request at {Path}", context.Request.Path);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = _env.IsDevelopment() ? ex.Message : "Something went wrong. Please try again later.",
                    Details = _env.IsDevelopment() ? ex.StackTrace : null
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}

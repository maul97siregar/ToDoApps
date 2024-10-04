namespace ToDoApp.Api.Utils
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        //public async Task InvokeAsync(HttpContext context)
        //{
        //    try
        //    {
        //        await _next(context);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        await HandleExceptionAsync(context, ex);
        //    }
        //}
        //private Task HandleExceptionAsync(HttpContext context, Exception exception)
        //{
        //    // Return appropriate HTTP status codes and messages
        //}
    }

}

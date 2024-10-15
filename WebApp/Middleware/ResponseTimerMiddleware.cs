using System.Diagnostics;

namespace WebApp.Middleware;

public class ResponseTimerMiddleware(RequestDelegate next, ILogger<ResponseTimerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            logger.LogInformation("Responded in {}ms to {} '{}'", stopwatch.ElapsedMilliseconds, context.Request.Method, context.Request.Path);
        }
    }
}
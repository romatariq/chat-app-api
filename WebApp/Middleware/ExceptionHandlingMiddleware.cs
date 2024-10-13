using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using App.Domain.Exceptions;
using App.DTO.Public.v1;

namespace WebApp.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        finally
        {
            stopwatch.Stop();
            logger.LogInformation("Took {}ms to execute '{}'", stopwatch.ElapsedMilliseconds, context.Request.Path);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "An unexpected error occurred.");

        var response = exception switch
        {
            CustomUserBadInputException e => new ErrorResponse(HttpStatusCode.BadRequest, e.Message),
            _ => new ErrorResponse(HttpStatusCode.InternalServerError, "Internal server error. Please try again later.")
        };

        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}
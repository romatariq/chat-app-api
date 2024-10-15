using System.Net;
using System.Net.Mime;
using App.Domain.Exceptions;
using App.DTO.Public.v1;

namespace WebApp.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = exception switch
        {
            CustomUserBadInputException e => new ErrorResponse(HttpStatusCode.BadRequest, e.Message),
            _ => new ErrorResponse(HttpStatusCode.InternalServerError, "Internal server error. Please try again later.")
        };

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            logger.LogError(exception, "An unexpected error occurred.");
        }
        else
        {
            logger.LogWarning("An error occured: {}", exception.Message);
        }

        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}
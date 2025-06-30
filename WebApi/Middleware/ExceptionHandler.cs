using System.Text.Json;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Identity.Middleware;

public class ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger, ProblemDetailsFactory problemDetailsFactory)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred.");
            
            var response = context.Response;
            response.ContentType = "application/json";

            var statusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                ArgumentException => StatusCodes.Status400BadRequest,
                BadRequestException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
            
            response.StatusCode = statusCode; 
            
            var problemDetails = problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode,
                title: "Unhandled exception",
                ex.Message, 
                context.Request.Path);
            
            var result = JsonSerializer.Serialize(problemDetails, JsonSerializerOptions.Default);
            
            await response.WriteAsync(result);
        }
    }
}
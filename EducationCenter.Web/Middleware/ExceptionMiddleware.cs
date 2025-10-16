using EducationCenter.Web.Exceptions;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EducationCenter.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode = ex switch
            {
                BadRequestException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };

            var errorResponse = new
            {
                StatusCode = (int)statusCode,
                Error = ex.GetType().Name,
                Message = ex.Message,
                StackTrace = ex.StackTrace
            };

            // ✅ Log full details to Serilog
            _logger.LogError(ex,
                "Exception caught in middleware. StatusCode: {StatusCode}, ErrorType: {ErrorType}, Message: {Message}",
                (int)statusCode, ex.GetType().Name, ex.Message);

            // ✅ Write clean JSON response for client
            context.Response.StatusCode = (int)statusCode;
            var json = JsonSerializer.Serialize(new
            {
                StatusCode = (int)statusCode,
                Message = ex.Message
            });

            await context.Response.WriteAsync(json);
        }
    }
}

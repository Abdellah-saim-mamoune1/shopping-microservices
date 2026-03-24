
using Catalog.API.Dtos;
using System.Net;


public class UnifiedResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UnifiedResponseMiddleware> _logger;

    public UnifiedResponseMiddleware(RequestDelegate next, ILogger<UnifiedResponseMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
          
            context.Items["CorrelationId"] = Guid.NewGuid().ToString();

            await _next(context);

        
            if (context.Response.HasStarted) return;

            var correlationId = context.Items["CorrelationId"]?.ToString();
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                context.Response.ContentType = "application/json";
                var response = UApiResponderDto<object>.Unauthorized();
                await context.Response.WriteAsJsonAsync(response);
            }
            else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                context.Response.ContentType = "application/json";
                var response = UApiResponderDto<object>.Forbidden();
                await context.Response.WriteAsJsonAsync(response);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            var correlationId = context.Items["CorrelationId"]?.ToString();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = UApiResponderDto<object>.InternalServerError();
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
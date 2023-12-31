using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Net.Mime;

namespace WebUI.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        try
        {
            await _next(context);
        }

        catch
        {
            var contextFeaute = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeaute != null)
            {
                logger.LogError(contextFeaute.Error.Message);
            }
            
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = MediaTypeNames.Application.Json;
        }
    }
}

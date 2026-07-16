using System.Text.Json;

namespace DesafioVWFS.DependencyInjection;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            var correlationId = context.Items.TryGetValue("X-Correlation-ID", out var value) ? value?.ToString() : null;

            _logger.LogError(ex,
                "Unhandled exception on {RequestMethod} {RequestPath} | CorrelationId={CorrelationId}",
                context.Request.Method,
                context.Request.Path,
                correlationId);

            if (!context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    mensagem = "Erro interno no servidor",
                    correlationId
                }));
            }
        }
    }
}
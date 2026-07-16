using DesafioVWFS.Data;
using DesafioVWFS.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DesafioVWFS;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var swaggerVersion = builder.Configuration["ApiDocumentation:Version"] ?? "v1";
        var swaggerTitle = builder.Configuration["ApiDocumentation:Title"] ?? "DesafioVWFS API";

        builder.AddLoggingInfrastructure();
        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddSwaggerWithJwt(builder.Configuration);

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/swagger/{swaggerVersion}/swagger.json", $"{swaggerTitle} {swaggerVersion}");
            options.RoutePrefix = string.Empty;
        });

        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.Use(async (context, next) =>
        {
            Log.Information("HTTP {RequestMethod} {RequestPath} started | CorrelationId={CorrelationId}",
                context.Request.Method,
                context.Request.Path,
                context.Items.TryGetValue("X-Correlation-ID", out var correlationId) ? correlationId : null);

            await next();
        });
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate =
                "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms | CorrelationId={CorrelationId}";
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                if (httpContext.Items.TryGetValue("X-Correlation-ID", out var correlationId) && correlationId is not null)
                {
                    diagnosticContext.Set("CorrelationId", correlationId);
                }
            };
        });

        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DesafioDbContext>();
            db.Database.Migrate();
        }

        app.Run();
    }
}

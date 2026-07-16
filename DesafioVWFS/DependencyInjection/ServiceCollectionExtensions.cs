using DesafioVWFS.Application.Features.Clients;
using DesafioVWFS.Application.Features.Contracts;
using DesafioVWFS.Application.Features.Payments;
using DesafioVWFS.Application.Repositories;
using DesafioVWFS.Application.Services;
using DesafioVWFS.Application.Shared.Domain.Repositories;
using DesafioVWFS.Application.Shared.Domain.Services;
using DesafioVWFS.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DesafioVWFS.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DesafioDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));

        services.AddScoped<IContratoRepository, ContratoRepository>();
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();

        services.AddScoped<IContratoService, ContratoService>();
        services.AddScoped<IPagamentoService, PagamentoService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IPagamentoCalculoService, PagamentoCalculoService>();

        services.AddScoped<IContractsApplicationService, ContractsApplicationService>();
        services.AddScoped<IPaymentsApplicationService, PaymentsApplicationService>();
        services.AddScoped<IClientsApplicationService, ClientsApplicationService>();

        services.AddControllers();
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    public static WebApplicationBuilder AddLoggingInfrastructure(this WebApplicationBuilder builder)
    {
        var logsDirectory = Path.Combine(builder.Environment.ContentRootPath, "logs");
        Directory.CreateDirectory(logsDirectory);

        var logFilePath = Path.Combine(logsDirectory, "app-.txt");

        builder.Host.UseSerilog((context, services, loggerConfig) =>
        {
            loggerConfig
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    path: logFilePath,
                    rollingInterval: RollingInterval.Day,
                    shared: true,
                    retainedFileCountLimit: null,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
        });

        return builder;
    }
}

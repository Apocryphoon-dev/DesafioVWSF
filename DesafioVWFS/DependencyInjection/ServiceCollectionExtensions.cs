using DesafioVWFS.Application.Features.Clients;
using DesafioVWFS.Application.Features.Clients.GetSummary.Handlers;
using DesafioVWFS.Application.Features.Contracts;
using DesafioVWFS.Application.Features.Contracts.CreateContract.Handlers;
using DesafioVWFS.Application.Features.Contracts.CreateContract.Models;
using DesafioVWFS.Application.Features.Contracts.CreateContract.Validators;
using DesafioVWFS.Application.Features.Contracts.DeleteContract.Handlers;
using DesafioVWFS.Application.Features.Contracts.GetContract.UseCase;
using DesafioVWFS.Application.Features.Contracts.ListContracts.Handlers;
using DesafioVWFS.Application.Features.Payments;
using DesafioVWFS.Application.Features.Payments.GetPayment.Handlers;
using DesafioVWFS.Application.Features.Payments.InsertPayment.Handlers;
using DesafioVWFS.Application.Features.Payments.InsertPayment.Models;
using DesafioVWFS.Application.Features.Payments.InsertPayment.Validators;
using DesafioVWFS.Application.Features.Payments.ListPayments.Handlers;
using DesafioVWFS.Application.Shared.Repository;
using DesafioVWFS.Application.Shared.Services;
using DesafioVWFS.Data;
using FluentValidation;
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

        services.AddScoped<IPagamentoCalculoService, PagamentoCalculoService>();

        services.AddScoped<CreateContractUseCase>();
        services.AddScoped<GetContractUseCase>();
        services.AddScoped<ListContractsUseCase>();
        services.AddScoped<DeleteContractUseCase>();
        services.AddScoped<InsertPaymentUseCase>();
        services.AddScoped<ListPaymentsUseCase>();
        services.AddScoped<GetPaymentUseCase>();
        services.AddScoped<GetSummaryUseCase>();

        services.AddScoped<IValidator<CreateContractInput>, CreateContractInputValidator>();
        services.AddScoped<IValidator<InsertPaymentInput>, InsertPaymentInputValidator>();

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

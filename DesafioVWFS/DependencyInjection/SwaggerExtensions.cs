using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

namespace DesafioVWFS.DependencyInjection;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var docsSection = configuration.GetSection("ApiDocumentation");
        var title = docsSection["Title"] ?? "DesafioVWFS API";
        var version = docsSection["Version"] ?? "v1";
        var description = docsSection["Description"] ?? "API para gestao de contratos, clientes e pagamentos.";
        var contactName = docsSection["ContactName"] ?? "Ricardo Cardoso";
        var contactEmail = docsSection["ContactEmail"] ?? "ricardo.rc15@hotmail.com";

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(version, new OpenApiInfo
            {
                Title = title,
                Version = version,
                Description = description,
                Contact = new OpenApiContact
                {
                    Name = contactName,
                    Email = contactEmail
                }
            });

            var bearerScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Informe o token no formato: Bearer {token}",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            options.AddSecurityDefinition("Bearer", bearerScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    bearerScheme,
                    Array.Empty<string>()
                }
            });
        });

        services.AddAuthentication("FixedToken")
            .AddScheme<AuthenticationSchemeOptions, FixedTokenAuthenticationHandler>("FixedToken", _ => { });

        services.AddAuthorization();

        return services;
    }
}

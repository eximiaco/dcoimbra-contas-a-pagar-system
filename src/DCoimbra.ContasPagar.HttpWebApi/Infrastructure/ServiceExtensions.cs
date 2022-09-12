using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DCoimbra.ContasPagar.HttpWebApi.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

        // TODO : Configurar Health Check
        /*hcBuilder
            .AddSqlServer(
                configuration["ConnectionStrings:Tenants"],
                name: "financeiro-check",
                tags: new string[] { "FinanceiroDbCheck" });*/

        return services;
    }

    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder =>
                    builder
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins(configuration.GetValue("AllowedOrigins", "*")));
        });
        return services;
    }
}
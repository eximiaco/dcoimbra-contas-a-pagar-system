using Autofac;
using Autofac.Extensions.DependencyInjection;
using DCoimbra.ContasPagar.HttpWebApi.Infrastructure;
using DCoimbra.ContasPagar.Infra;
using Microsoft.API.Infraestrutura.Filtros;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;

var appNamespace = typeof(Program).Namespace ?? throw new ArgumentNullException($"typeof(Program).Namespace");
var appName = appNamespace?[(appNamespace.LastIndexOf('.', appNamespace.LastIndexOf('.') - 1) + 1)..];
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")??"Development";

var builder = WebApplication.CreateBuilder(args);
// TODO : Adicionar configuracao para cofres (aka AzureKeyVault)
var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();
var configuration = configBuilder.Build();

var loggerSwitch = new LoggingLevelSwitch
{
    MinimumLevel = Enum.TryParse(configuration["LogSwitch"], true, out LogEventLevel level) 
        ? level 
        : LogEventLevel.Warning
};
var loggerBuilder = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .MinimumLevel.ControlledBy(loggerSwitch)
    .Enrich.WithProperty("Environment", environmentName)
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .Enrich.WithMachineName();
Log.Logger = loggerBuilder.CreateLogger();

try
{
    Log
        .ForContext("ApplicationName", appName)
        .Information("Starting application");
    
    builder.Services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.Name));
    builder.Services
        .AddControllers(options =>
        {
            options.Filters.Add(typeof(HttpGlobalExceptionFilter));
        });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddOptions();
    builder.Services
        .AddCustomCors(configuration)
        .AddHealthChecks();

    builder.Host.UseSerilog();
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new MediatorModule()));
    builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ApplicationModule()));
        
    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        // TODO: Debater uso em DEV ou PROD
        app.UseSerilogRequestLogging();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors("CorsPolicy");
    app.UseHttpsRedirection();
    // TODO : Configurar Autorização
    //app.UseAuthorization();
    app.MapControllers();
    // TODO : Configurar Health Checks
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log
        .ForContext("ApplicationName", appName)
        .Fatal(ex, "Program terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Polly;
using WonderFood.Application;
using WonderFood.ExternalServices;
using WonderFood.MySql;
using WonderFood.MySql.Context;
using Serilog;

namespace WonderFood.Worker;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        services.Configure<ExternalServicesSettings>(Configuration.GetSection("ExternalServicesSettings"));
        services.AddSqlInfrastructure(Configuration);
        services.AddApplication();
        services.AddExternalServices();
        services.AddEndpointsApiExplorer();
        services.AddSwagger();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WonderfoodContext dbContext)
    {
        app.UseSwaggerMiddleware();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        ExecuteDatabaseMigration(dbContext);
    }

    private void ExecuteDatabaseMigration(WonderfoodContext dbContext)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(new[]
                {
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4),
                    TimeSpan.FromSeconds(8),
                    TimeSpan.FromSeconds(16),
                    TimeSpan.FromSeconds(32),
                },
                (_, timeSpan, retryCount, _) =>
                {
                    Log.Logger.Error("Tentativa {0} de conexão ao MySql falhou. Tentando novamente em {1} segundos.",
                        retryCount, timeSpan.Seconds);
                });
        retryPolicy.Execute(() => { dbContext.Database.Migrate(); });
    }
}
using System.Text.Json.Serialization;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Polly;
using Serilog;
using WonderFood.Application;
using WonderFood.MySql;
using WonderFood.MySql.Context;
using WonderFood.Worker.Consumers;

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
        
        services.AddSqlInfrastructure(Configuration);
        services.AddApplication();
        services.AddEndpointsApiExplorer();
        services.AddSwagger();

        var rabbitMqUser = Configuration["RABBITMQ_DEFAULT_USER"];
        var rabbitMqPassword = Configuration["RABBITMQ_DEFAULT_PASS"];
        var rabbitMqHost = Configuration["RABBITMQ_HOST"];

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<IniciarProducaoConsumer>();
            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqHost, hst =>
                {
                    hst.Username(rabbitMqUser);
                    hst.Password(rabbitMqPassword);
                });

                cfg.ReceiveEndpoint("iniciar_producao", e =>
                {
                    e.ConfigureConsumer<IniciarProducaoConsumer>(context);
                    e.Bind("WonderFood.Models.Events:PagamentoProcessadoEvent", x =>
                    {
                        x.RoutingKey = "pagamento.processado";
                        x.ExchangeType = "fanout";
                    });
                });
            });
        });
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
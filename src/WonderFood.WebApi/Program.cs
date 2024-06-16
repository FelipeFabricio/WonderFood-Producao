using System.Text.Json.Serialization;
using MassTransit;
using WonderFood.Application;
using WonderFood.Models.Events;
using WonderFood.MySql;
using WonderFood.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddSqlInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var rabbitMqUser = builder.Configuration["RABBITMQ_DEFAULT_USER"];
var rabbitMqPassword = builder.Configuration["RABBITMQ_DEFAULT_PASS"];
var rabbitMqHost = builder.Configuration["RABBITMQ_HOST"];

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.UsingRabbitMq((_, cfg) =>
    {
        cfg.Host(rabbitMqHost, hst =>
        {
            hst.Username(rabbitMqUser);
            hst.Password(rabbitMqPassword);
        });
                
        cfg.Publish<StatusPedidoAlteradoEvent>(x =>
        {
            x.ExchangeType = "fanout";
        });
    });
});

var app = builder.Build();

app.UseSwaggerMiddleware();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

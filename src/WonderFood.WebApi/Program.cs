using System.Text.Json.Serialization;
using WonderFood.Application;
using WonderFood.ExternalServices;
using WonderFood.MySql;
using WonderFood.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.Configure<ExternalServicesSettings>(builder.Configuration.GetSection("ExternalServicesSettings"));
builder.Services.AddSqlInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddExternalServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();

app.UseSwaggerMiddleware();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

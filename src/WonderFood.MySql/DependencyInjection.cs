using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using WonderFood.Application.Common;
using WonderFood.MySql.Context;
using WonderFood.MySql.Repositories;

namespace WonderFood.MySql;

public static class DependencyInjection
{
    public static void AddSqlInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var enviroment = configuration["ASPNETCORE_ENVIRONMENT"];
        var connectionString = enviroment == "Development" ? configuration["ConnectionStrings:DefaultConnection"] : 
            configuration["MYSQL_CONNECTION"];
            
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 36));
        services.AddDbContext<WonderfoodContext>(
            dbContextOptions => dbContextOptions.UseMySql(connectionString, serverVersion,
                mySqlOptions => { mySqlOptions.EnableRetryOnFailure(); }));
            
        services.AddScoped<IPedidoRepository, PedidoRepository>();
    }
}
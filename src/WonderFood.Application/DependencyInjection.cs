using Microsoft.Extensions.DependencyInjection;
using WonderFood.Application.Common;
using WonderFood.Application.Services;

namespace WonderFood.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPedidoService, PedidoService>();
    }
}
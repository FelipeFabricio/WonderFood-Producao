using Microsoft.Extensions.DependencyInjection;
using WonderFood.Application.Common;
using WonderFood.ExternalServices.ExternalServices;

namespace WonderFood.ExternalServices;

public static class DependencyInjection
{
    public static void AddExternalServices(this IServiceCollection services)
    {
        services.AddScoped<IWonderfoodPedidosExternal, WonderfoodPedidosExternal>();
    }
}
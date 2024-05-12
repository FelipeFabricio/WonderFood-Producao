using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using WonderFood.Application.Common;
using WonderFood.ExternalServices.ExternalServices;


namespace WonderFood.ExternalServices;

public static class DependencyInjection
{
    public static void AddExternalServices(this IServiceCollection services)
    {
        services.AddHttpClient<IWonderfoodPedidosExternal, WonderfoodPedidosExternal>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        
        services.AddScoped<IWonderfoodPedidosExternal, WonderfoodPedidosExternal>();
    }
}
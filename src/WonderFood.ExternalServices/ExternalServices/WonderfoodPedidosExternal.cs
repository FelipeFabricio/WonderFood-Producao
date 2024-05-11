using Microsoft.Extensions.Options;
using WonderFood.Application.Common;
using WonderFood.Domain.Entities.Enums;

namespace WonderFood.ExternalServices.ExternalServices;

public class WonderfoodPedidosExternal(HttpClient httpClient, IOptions<ExternalServicesSettings> settings) :
    ExternalServiceBase, IWonderfoodPedidosExternal
{
    private readonly ExternalServicesSettings _settings = settings.Value;

    public async Task ComunicarAteracaoStatus(int numeroPedido, StatusPedido status)
    {
        var alteracaoStatus = new
        {
            NumeroPedido = numeroPedido,
            Status = status
        };
        var url = string.Concat(_settings.WonderfoodPedidos.BaseUrl, _settings.WonderfoodPedidos.AlteracaoStatus);
        using var response = await httpClient.PostAsync(url, CreateStringContent(alteracaoStatus));
    }
}
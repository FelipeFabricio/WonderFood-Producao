using WonderFood.Domain.Entities.Enums;

namespace WonderFood.Application.Common;

public interface IWonderfoodPedidosExternal
{
    Task ComunicarAteracaoStatus(int numeroPedido, StatusPedido status);
}
using WonderFood.Domain.Entities;
using WonderFood.Domain.Entities.Enums;

namespace WonderFood.Application.Common;

public interface IPedidoRepository
{
    Task AlterarStatus(Guid idPedido, StatusPedido status);
    Task Inserir(Pedido pedido);
}
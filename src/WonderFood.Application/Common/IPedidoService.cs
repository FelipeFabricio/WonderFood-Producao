using WonderFood.Domain.Entities.Enums;
using WonderFood.Models.Events;

namespace WonderFood.Application.Common;

public interface IPedidoService
{
    Task ReceberPedidosParaPreparo(IniciarProducaoCommand pedido);
    Task AlterarStatusPedido(int numeroPedido, StatusPedido status, string motivoCancelamento);
}
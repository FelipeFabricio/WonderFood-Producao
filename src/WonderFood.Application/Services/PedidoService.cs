using WonderFood.Application.Common;
using WonderFood.Domain.Entities;
using WonderFood.Domain.Entities.Enums;
using WonderFood.Models.Events;

namespace WonderFood.Application.Services;

public class PedidoService(IPedidoRepository pedidoRepository, IWonderfoodPedidosExternal pedidosExternal) : IPedidoService
{
    public async Task ReceberPedidosParaPreparo(IniciarProducaoCommand pedido)
    {
        var produtos = pedido.Produtos.Select(p => new Domain.Entities.ProdutosPedido
        {
            ProdutoId = p.ProdutoId, 
            Quantidade = p.Quantidade, 
            PedidoId = pedido.IdPedido,
        });
        
        var pedidoEntity = new Pedido(pedido.IdPedido, StatusPedido.AguardandoPreparo, pedido.NumeroPedido,
            pedido.Observacao, produtos);
        
        await pedidoRepository.Inserir(pedidoEntity);
    }

    public async Task AlterarStatusPedido(int numeroPedido, StatusPedido status)
    {
        await pedidoRepository.AlterarStatus(numeroPedido, status);
        await pedidosExternal.ComunicarAteracaoStatus(numeroPedido, status);
    }
}
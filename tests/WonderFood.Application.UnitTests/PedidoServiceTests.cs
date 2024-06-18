using MassTransit;
using NSubstitute;
using WonderFood.Application.Common;
using WonderFood.Application.Services;
using WonderFood.Domain.Entities;
using WonderFood.Domain.Entities.Enums;
using WonderFood.Models.Events;
using ProdutosPedido = WonderFood.Domain.Entities.ProdutosPedido;

namespace WonderFood.Application.UnitTests;

public class PedidoServiceTests
{
    private readonly PedidoService _sut;
    private readonly IPedidoRepository _pedidoRepository = Substitute.For<IPedidoRepository>();
    private readonly IBus _bus = Substitute.For<IBus>();

    public PedidoServiceTests()
    {
        _sut = new PedidoService(_pedidoRepository,_bus);
    }
    
    [Fact]
    [Trait("Application", "Pedido")]
    public async Task AlterarStatusPedido_DeveComunicarAlteracaoStatus_QuandoStatusAlteradoComSucesso()
    {
        //Arrange
        var pedido = new Pedido(Guid.NewGuid(), StatusPedido.AguardandoPreparo, 1, "Sem cebola", new List<ProdutosPedido>
        {
            new()
            {
                ProdutoId = Guid.NewGuid(),
                Quantidade = 2
            }
        });
        _pedidoRepository.AlterarStatus(Arg.Any<int>(), Arg.Any<StatusPedido>()).Returns(Task.CompletedTask);
        _pedidoRepository.ObterPorNumeroPedido(Arg.Any<int>()).Returns(pedido);
        
        //Act
        await _sut.AlterarStatusPedido(1, StatusPedido.PreparoIniciado, null);
        
        //Assert
        await _pedidoRepository.Received(1).AlterarStatus(Arg.Any<int>(), Arg.Any<StatusPedido>());
        await _bus.Received(1).Publish(Arg.Any<StatusPedidoAlteradoEvent>());
        
    }
    
}
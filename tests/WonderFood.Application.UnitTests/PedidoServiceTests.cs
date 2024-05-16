using NSubstitute;
using WonderFood.Application.Common;
using WonderFood.Application.Services;
using WonderFood.Domain.Entities.Enums;

namespace WonderFood.Application.UnitTests;

public class PedidoServiceTests
{
    private readonly PedidoService _sut;
    private readonly IPedidoRepository _pedidoRepository = Substitute.For<IPedidoRepository>();
    private readonly IWonderfoodPedidosExternal _pedidosExternal = Substitute.For<IWonderfoodPedidosExternal>();

    public PedidoServiceTests()
    {
        _sut = new PedidoService(_pedidoRepository, _pedidosExternal);
    }
    
    [Fact]
    [Trait("Application", "Pedido")]
    public async Task AlterarStatusPedido_DeveComunicarAlteracaoStatus_QuandoStatusAlteradoComSucesso()
    {
        //Arrange
        
        _pedidoRepository.AlterarStatus(Arg.Any<int>(), Arg.Any<StatusPedido>()).Returns(Task.CompletedTask);
        _pedidosExternal.ComunicarAteracaoStatus(Arg.Any<int>(), Arg.Any<StatusPedido>()).Returns(Task.CompletedTask);
        
        //Act
        await _sut.AlterarStatusPedido(1, StatusPedido.PreparoIniciado);
        
        //Assert
        await _pedidoRepository.Received(1).AlterarStatus(Arg.Any<int>(), Arg.Any<StatusPedido>());
        await _pedidosExternal.Received(1).ComunicarAteracaoStatus(Arg.Any<int>(), Arg.Any<StatusPedido>());
    }
    
}
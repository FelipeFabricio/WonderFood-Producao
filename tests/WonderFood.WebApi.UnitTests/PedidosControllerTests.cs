using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WonderFood.Application.Common;
using WonderFood.Domain.Entities.Enums;
using WonderFood.WebApi.Controllers;
using WonderFood.WebApi.Tests.Fixture;

namespace WonderFood.WebApi.Tests;

[Collection(nameof(PedidoFixtureCollection))]
public class PedidosControllerTests
{
    private readonly PedidosController _sut;
    private readonly IPedidoService _pedidoService = Substitute.For<IPedidoService>();
    private const int NumeroPedido = 1;
    private const StatusPedido Status = StatusPedido.PagamentoAprovado;

    public PedidosControllerTests()
    {
        _sut = new PedidosController(_pedidoService);
    }
    
    [Fact]
    [Trait("WebApi", "Pedido")]
    public async Task AlterarStatusPedido_DeveRetornarOk_QuandoAlteracaoForEfetuadaComSucesso()
    {
        //Arrange
        _pedidoService.AlterarStatusPedido(NumeroPedido, Status).Returns(Task.CompletedTask);

        //Act
        var resultado = (OkResult)await _sut.AlterarStatusPedido(NumeroPedido, Status);
    
        //Assert
        resultado.StatusCode.Should().Be(200);
    }
    
    [Fact]
    [Trait("WebApi", "Pedido")]
    public async Task AlterarStatusPedido_DeveRetornarBadRequest_QuandoUmaExceptionForLancada()
    {
        //Arrange
        _pedidoService.AlterarStatusPedido(Arg.Any<int>(), Arg.Any<StatusPedido>()).Throws(new Exception());

        //Act
        var resultado = (BadRequestObjectResult)await _sut.AlterarStatusPedido(NumeroPedido, Status);
    
        //Assert
        resultado.StatusCode.Should().Be(400);
    }
}
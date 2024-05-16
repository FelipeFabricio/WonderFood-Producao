using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WonderFood.Application.Common;
using WonderFood.Domain.Entities.Enums;
using WonderFood.Models.Events;
using WonderFood.Worker.Webhooks;

namespace WonderFood.Worker.UnitTests;

public class PedidosWebhookTests
{
    private readonly IPedidoService _pedidoService = Substitute.For<IPedidoService>();
    private readonly PedidosWebhook _sut;

    public PedidosWebhookTests()
    {
        _sut = new PedidosWebhook(_pedidoService);
    }
    
    [Fact]
    [Trait("Worker", "PedidosWebhook")]
    public async Task IniciarProducaoPedido_DeveRetornarOk_QuandoAlteracaoForEfetuadaComSucesso()
    {
        //Arrange
        var producaoCommand = new IniciarProducaoCommand
        {
            NumeroPedido = 1,
            Status = StatusPedido.AguardandoPreparo
        };
        _pedidoService.ReceberPedidosParaPreparo(producaoCommand).Returns(Task.CompletedTask);

        //Act
        var resultado = (OkResult)await _sut.IniciarProducaoPedido(producaoCommand);
    
        //Assert
        resultado.StatusCode.Should().Be(200);
    }
    
    [Fact]
    [Trait("Worker", "PedidosWebhook")]
    public async Task IniciarProducaoPedido_DeveRetornarBadRequest_QuandoUmaExceptionForLancada()
    {
        //Arrange
        var producaoCommand = new IniciarProducaoCommand
        {
            NumeroPedido = 1,
            Status = StatusPedido.AguardandoPreparo
        };
        _pedidoService.ReceberPedidosParaPreparo(producaoCommand).Throws(new Exception());

        //Act
        var resultado = (BadRequestObjectResult)await _sut.IniciarProducaoPedido(producaoCommand);
    
        //Assert
        resultado.StatusCode.Should().Be(400);
    }
}
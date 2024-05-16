using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using TechTalk.SpecFlow;
using WonderFood.Application.Common;
using WonderFood.Application.Services;
using WonderFood.BDD.Tests.Common;
using WonderFood.Domain.Entities.Enums;
using WonderFood.Models.Events;
using WonderFood.MySql.Context;
using WonderFood.MySql.Repositories;

namespace WonderFood.BDD.Tests.Pedidos.Steps;

[Binding]
public class ReceberPedidoParaPreparoSteps
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IWonderfoodPedidosExternal _pedidosExternal = Substitute.For<IWonderfoodPedidosExternal>();
    private readonly WonderfoodContext _context;
    private readonly InMemoryDbContextFactory _dbContextFactory;
    private readonly ServiceProvider _serviceProvider;
    private readonly PedidoService _pedidoService;
    private IniciarProducaoCommand _producaoCommand;

    public ReceberPedidoParaPreparoSteps()
    {
        var serviceCollection = new ServiceCollection();
        _dbContextFactory = new InMemoryDbContextFactory();
        _context = _dbContextFactory.CreateContext();
        
        serviceCollection.AddSingleton(_context);
        serviceCollection.AddScoped<IPedidoRepository, PedidoRepository>();
        _serviceProvider = serviceCollection.BuildServiceProvider();
        _pedidoRepository = _serviceProvider.GetRequiredService<IPedidoRepository>();
        
        _pedidoService = new PedidoService(_pedidoRepository, _pedidosExternal);
    }
    
    [Given(@"que o Cliente efetua um pedido")]
    public void GivenQueOClienteEfetuaUmPedido()
    {
        _producaoCommand = new IniciarProducaoCommand
        {
            IdPedido = Guid.NewGuid(),
            NumeroPedido = 1,
            Observacao = "Sem cebola",
            Produtos = new List<ProdutosPedido>
            {
                new()
                {
                    ProdutoId = Guid.NewGuid(),
                    Quantidade = 2
                }
            }
        };
    }

    [Given(@"esse Pedido possui pelo menos um produto")]
    public void GivenEssePedidoPossuiPeloMenosUmProduto()
    {
        //Assert
        Assert.NotEmpty(_producaoCommand.Produtos);
    }

    [When(@"recebermos a comunicação do Pedido")]
    public async Task WhenRecebermosAComunicacaoDoPedido()
    {
        await _pedidoService.ReceberPedidosParaPreparo(_producaoCommand);
    }

    [Then(@"ele deve ser inserido na nossa base de dados para ser possível iniciar o preparo")]
    public async Task ThenEleDeveSerInseridoNaNossaBaseDeDadosParaSerPossivelIniciarOPreparo()
    {
        //Assert
        var pedido = await _pedidoRepository.ObterPorNumeroPedido(_producaoCommand.NumeroPedido);
        Assert.NotNull(pedido);
        Assert.Equal(StatusPedido.AguardandoPreparo, pedido.Status);
    }
}
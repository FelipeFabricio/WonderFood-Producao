using Bogus;
using WonderFood.Domain.Entities;
using WonderFood.Domain.Entities.Enums;

namespace WonderFood.Domain.UnitTests;

public class PedidoTests
{
    [Theory]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    [Trait("Domain", "Pedido")]
    public void PreencherProdutosPedido_DeveLancarArgumentException_QuandoProdutosInvalidos(int quantidadeProdutos,
        int quantidadeItensLista)
    {
        // Arrange
        var listaProdutos = GerarListaProdutosPedido(quantidadeItensLista, quantidadeProdutos);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Pedido(Guid.NewGuid(), StatusPedido.AguardandoPreparo, 1, "Obs", listaProdutos));
    }

    [Theory]
    [InlineData(StatusPedido.AguardandoPagamento)]
    [InlineData(StatusPedido.PreparoIniciado)]
    [InlineData(StatusPedido.ProntoParaRetirada)] 
    [Trait("Domain", "Pedido")]
    public void PreencherStatusPedido_DeveLancarArgumentException_QuandoStatusInvalido(StatusPedido statusPedido)
    {
        // Arrange
        var listaProdutos = GerarListaProdutosPedido();
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Pedido(Guid.NewGuid(), statusPedido, 1, "Obs", listaProdutos));
    }

    public IEnumerable<ProdutosPedido> GerarListaProdutosPedido(int quantidadeItensLista = 1,
        int quantidadeProdutos = 1)
    {
        return new Faker<ProdutosPedido>("pt_BR")
            .RuleFor(c => c.Quantidade, f => quantidadeProdutos)
            .RuleFor(c => c.ProdutoId, f => f.Random.Guid())
            .RuleFor(c => c.PedidoId, f => f.Random.Guid())
            .RuleFor(c => c.Produto, new Produto
            {
                Nome = "Nome",
                Valor = 10M,
                Categoria = CategoriaProduto.Acompanhamento,
                Descricao = "Descricao"
            })
            .Generate(quantidadeItensLista);
    }
}
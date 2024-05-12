using WonderFood.Domain.Entities.Enums;

namespace WonderFood.Domain.Entities;

public class Pedido
{
    public Guid Id { get; }
    public int NumeroPedido { get; set; }
    public string? Observacao { get; init; }
    public DateTime DataEntrada { get; init; }
    public DateTime? DataSaída { get; init; }
    public StatusPedido Status { get; private set; }
    public IEnumerable<ProdutosPedido> Produtos { get; private set; }


    public Pedido(Guid id, StatusPedido status, int numeroPedido, string? observacao,IEnumerable<ProdutosPedido> produtos)
    {
        Id = id;
        DataEntrada = DateTime.Now;
        NumeroPedido = numeroPedido;
        Observacao = observacao;
        PreencherStatusPedido(status);
        PreencherProdutosPedido(produtos);
    }

    private void PreencherProdutosPedido(IEnumerable<ProdutosPedido> produtos)
    {
        var produtosPedidos = produtos.ToList();
        if (!produtosPedidos.Any())
            throw new ArgumentException("A lista de produtos não pode ser vazia.");

        if (produtosPedidos.Exists(p => p.Quantidade <= 0))
            throw new ArgumentException("A quantidade de produtos não pode ser menor ou igual a zero.");
        
        foreach (var produto in produtosPedidos)
            produto.PedidoId = Id;
        
        Produtos = produtosPedidos;
    }
    
    private void PreencherStatusPedido(StatusPedido status)
    {
        if(status != StatusPedido.AguardandoPreparo)
            throw new InvalidOperationException("Status do pedido inválido");
        
        Status = status;
    }
    
    private Pedido()
    {
        // EF Core
    }
}
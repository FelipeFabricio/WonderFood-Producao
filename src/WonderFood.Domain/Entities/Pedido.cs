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
        if(produtos is null || !produtos.Any())
            throw new InvalidOperationException("Pedido sem produtos");

        Produtos = produtos;
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
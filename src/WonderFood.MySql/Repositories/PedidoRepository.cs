using Microsoft.EntityFrameworkCore;
using WonderFood.Application.Common;
using WonderFood.Domain.Entities;
using WonderFood.Domain.Entities.Enums;
using WonderFood.MySql.Context;

namespace WonderFood.MySql.Repositories;

public class PedidoRepository(WonderfoodContext context) : IPedidoRepository
{
    public async Task AlterarStatus(int numeroPedido, StatusPedido status)
    {
        await context.Pedidos.Where(p => p.NumeroPedido == numeroPedido)
            .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.Status, status));
    }

    public async Task Inserir(Pedido pedido)
    {
        await context.Pedidos.AddAsync(pedido);
        await context.SaveChangesAsync();
    }

    public async Task<Pedido?> ObterPorNumeroPedido(int numeroPedido)
    {
        var pedido =  await context.Pedidos
            .Include(p => p.Produtos)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.NumeroPedido == numeroPedido);
        return pedido;
    }
}
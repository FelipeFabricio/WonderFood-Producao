using Microsoft.EntityFrameworkCore;
using WonderFood.Application.Common;
using WonderFood.Domain.Entities;
using WonderFood.Domain.Entities.Enums;
using WonderFood.MySql.Context;

namespace WonderFood.MySql.Repositories;

public class PedidoRepository(WonderfoodContext context) : IPedidoRepository
{
    public async Task AlterarStatus(Guid idPedido, StatusPedido status)
    {
        await context.Pedidos.Where(p => p.Id == idPedido)
            .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.Status, status));
    }

    public async Task Inserir(Pedido pedido)
    {
        await context.Pedidos.AddAsync(pedido);
        await context.SaveChangesAsync();
    }
}
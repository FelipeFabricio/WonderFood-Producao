using Microsoft.EntityFrameworkCore;
using WonderFood.Domain.Entities;

namespace WonderFood.MySql.Context;

public class WonderfoodContext : DbContext
{
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<ProdutosPedido> ProdutosPedido { get; set; }

    public WonderfoodContext(DbContextOptions<WonderfoodContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WonderfoodContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
using Microsoft.EntityFrameworkCore;
using WonderFood.Domain.Entities;
using WonderFood.Domain.Entities.Enums;

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
        
        modelBuilder.Entity<Produto>().HasData(
            new Produto
            {
                Id = Guid.Parse("e5d62425-d113-46ce-8769-58b07133d92b"),
                Nome = "Hamburguer",
                Descricao = "Top demais",
                Valor = 10.90m,
                Categoria = CategoriaProduto.Lanche
            }
        );
        
        base.OnModelCreating(modelBuilder);
    }
}
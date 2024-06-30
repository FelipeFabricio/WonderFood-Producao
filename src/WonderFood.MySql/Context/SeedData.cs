using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WonderFood.Domain.Entities;
using WonderFood.Domain.Entities.Enums;

namespace WonderFood.MySql.Context;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new WonderfoodContext(
                   serviceProvider.GetRequiredService<DbContextOptions<WonderfoodContext>>()))
        {
            if (context.Produtos.Any())
                return;
            
                
            context.Produtos.AddRange(
                new Produto
                {
                    Id = Guid.Parse("e5d62425-d113-46ce-8769-58b07133d92b"),
                    Nome = "Hamburguer",
                    Descricao = "Top demais",
                    Valor = 10.90m,
                    Categoria = CategoriaProduto.Lanche
                }
            );
            context.SaveChanges();
        }
    }
}
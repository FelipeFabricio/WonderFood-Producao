using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WonderFood.Domain.Entities;

namespace WonderFood.MySql.Mappings
{
    public class PedidoDatabaseMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("varchar(36)").IsRequired();
            builder.Property(p => p.Status).HasConversion<byte>().IsRequired();
            builder.Property(p => p.Observacao).HasColumnType("varchar(200)").IsRequired(false);
            builder.HasIndex(p => p.NumeroPedido).IsUnique();
            builder.Property(p => p.NumeroPedido).HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
        }
    }
}
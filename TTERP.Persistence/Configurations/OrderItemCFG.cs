using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TTERP.Domain.Entities;

namespace TTERP.Persistence.Configurations
{
    public class OrderItemCFG : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            //builder.HasKey(oi => new {oi.OrderId, oi.ProductId});

            builder.HasIndex(oi => new { oi.OrderId, oi.ProductId })
                   .IsUnique(); // haskey yerine unique constraint tanýmladým çünkü orderItemWarehouse tablosuyla iliþki kurarken foreign key hatasý aldým. orderItem ile warehouse arasýndaki manytomany iliþkinin ara tablosu orderItemWarehouse. foreign key olarak orderItemId almaya çalýþýyorum fakat burada composite key tanýmý olduðu için patlýyor. mecburen tekil id kullanacaðým orderItemda. 

            builder.Property(oi => oi.OrderId)
                   .IsRequired();

            builder.Property(oi => oi.ProductId)
                   .IsRequired();

            builder.HasIndex(x => new { x.OrderId, x.ProductId })
                   .IsUnique();

            builder.Property(oi => oi.Quantity)
                   .IsRequired()
                   .HasDefaultValue(1);

            builder.Property(oi => oi.TaxRate)
                   .IsRequired()
                   .HasColumnType("decimal(5,2)");

            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Product)
                   .WithMany(p => p.OrderItems)
                   .HasForeignKey(oi => oi.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(oi => oi.OrderItemWarehouses)
                   .WithOne(oiw => oiw.OrderItem)
                   .HasForeignKey(oiw => oiw.OrderItemId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
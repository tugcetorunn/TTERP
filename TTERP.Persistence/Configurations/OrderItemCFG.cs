using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TTERP.Domain.Entities;

namespace TTERP.Persistence.Configurations
{
    public class OrderItemCFG : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(oi => oi.OrderId)
                   .IsRequired();

            builder.Property(oi => oi.ProductId)
                   .IsRequired();

            builder.HasIndex(x => new { x.OrderId, x.ProductId })
                   .IsUnique();

            builder.Property(oi => oi.Quantity)
                   .IsRequired()
                   .HasDefaultValue(1);

            builder.Property(oi => oi.UnitPrice)
                   .IsRequired()
                   .HasColumnType("money");

            builder.Property(oi => oi.Discount)
                   .IsRequired()
                   .HasColumnType("money")
                   .HasDefaultValue(0m);

            builder.Property(oi => oi.Tax)
                   .IsRequired()
                   .HasColumnType("money")
                   .HasDefaultValue(0m);

            builder.Property(oi => oi.TotalPrice)
                   .IsRequired()
                   .HasColumnType("money");

            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Product)
                   .WithMany(p => p.OrderItems)
                   .HasForeignKey(oi => oi.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
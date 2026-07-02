using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Persistence.Configurations
{
    public class ProductCFG : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.Description)
                .IsRequired(false)
                .HasMaxLength(200);

            builder.Property(p => p.CostPrice)
                .IsRequired()
                .HasColumnType("decimal(18,4)");

            builder.Property(p => p.TaxRate)
                   .IsRequired()
                   .HasColumnType("decimal(5,2)");

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.ProductWarehouses)
                   .WithOne(pw => pw.Product)
                   .HasForeignKey(pw => pw.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

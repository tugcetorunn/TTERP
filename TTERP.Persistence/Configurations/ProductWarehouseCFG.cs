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
    public class ProductWarehouseCFG : IEntityTypeConfiguration<ProductWarehouse>
    {
        public void Configure(EntityTypeBuilder<ProductWarehouse> builder)
        {
            //builder.HasKey(pw => new { pw.ProductId, pw.WarehouseId });
            
            builder.HasIndex(pw => new { pw.ProductId, pw.WarehouseId })
                   .IsUnique();

            builder.HasOne(pw => pw.Product)
                   .WithMany(p => p.ProductWarehouses)
                   .HasForeignKey(pw => pw.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pw => pw.Warehouse)
                   .WithMany(w => w.ProductWarehouses)
                   .HasForeignKey(pw => pw.WarehouseId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

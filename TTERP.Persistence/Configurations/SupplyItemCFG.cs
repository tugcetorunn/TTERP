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
    public class SupplyItemCFG : IEntityTypeConfiguration<SupplyItem>
    {
        public void Configure(EntityTypeBuilder<SupplyItem> builder)
        {
            builder.HasOne(x => x.Supply)
                   .WithMany(x => x.SupplyItems)
                   .HasForeignKey(x => x.SupplyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Material)
                   .WithMany(x => x.SupplyItems)
                   .HasForeignKey(x => x.MaterialId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Warehouse)
                   .WithMany(x => x.SupplyItems)
                   .HasForeignKey(x => x.WarehouseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.TaxRate)
                   .IsRequired()
                   .HasColumnType("decimal(5,2)");

            builder.HasIndex(x => new { x.MaterialId, x.WarehouseId })
                   .IsUnique();
        }
    }
}

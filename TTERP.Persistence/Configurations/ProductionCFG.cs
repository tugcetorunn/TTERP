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
    public class ProductionCFG : IEntityTypeConfiguration<Production>
    {
        public void Configure(EntityTypeBuilder<Production> builder)
        {
            builder.Property(p => p.ProductionStatus)
                   .IsRequired()
                   .HasDefaultValue(1);

            builder.HasOne(p => p.Product)
                   .WithMany(pr => pr.Productions)
                   .HasForeignKey(p => p.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.TargetWarehouse) // hedef depo - bitmiş ürün girişinde (kaynak depo prop unu kldırdık çünkü kaynak depo prod itemlara bağlı yani birden fazla farklı depodan gelebilir.)
                   .WithMany(w => w.TargetProductions) 
                   .HasForeignKey(p => p.TargetWarehouseId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

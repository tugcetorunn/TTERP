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
    public class ProductionItemCFG : IEntityTypeConfiguration<ProductionItem>
    {
        public void Configure(EntityTypeBuilder<ProductionItem> builder)
        {
            //builder.HasKey(pi => new { pi.ProductionId, pi.MaterialId });

            builder.HasIndex(pi => new { pi.ProductionId, pi.MaterialId })
                   .IsUnique();

            builder.HasOne(pi => pi.Production)
                   .WithMany(p => p.ProductionItems)
                   .HasForeignKey(pi => pi.ProductionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pi => pi.Material)
                   .WithMany(p => p.ProductionItems)
                   .HasForeignKey(pi => pi.MaterialId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

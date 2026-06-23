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
    public class SupplierMaterialCFG : IEntityTypeConfiguration<SupplierMaterial>
    {
        public void Configure(EntityTypeBuilder<SupplierMaterial> builder)
        {
            builder.HasKey(sm => new { sm.SupplierId, sm.MaterialId });

            builder.HasOne(sm => sm.Supplier)
               .WithMany(s => s.SupplierMaterials)
               .HasForeignKey(sm => sm.SupplierId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sm => sm.Material)
                .WithMany(m => m.SupplierMaterials)
                .HasForeignKey(sm => sm.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

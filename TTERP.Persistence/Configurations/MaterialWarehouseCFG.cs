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
    public class MaterialWarehouseCFG : IEntityTypeConfiguration<MaterialWarehouse>
    {
        public void Configure(EntityTypeBuilder<MaterialWarehouse> builder)
        {
            // many to many ilişkide oluşturulan ara tablonun cfg sinde her iki taraftaki ilişkiyi de eklemeliyiz.
            builder.HasOne(x => x.Material)
                   .WithMany(x => x.MaterialWarehouses)
                   .HasForeignKey(x => x.MaterialId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Warehouse)
                   .WithMany(x => x.MaterialWarehouses)
                   .HasForeignKey(x => x.WarehouseId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

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
    public class OrderItemWarehouseCFG : IEntityTypeConfiguration<OrderItemWarehouse>
    {
        public void Configure(EntityTypeBuilder<OrderItemWarehouse> builder)
        {
            //builder.HasKey(oiw => new { oiw.OrderItemId, oiw.WarehouseId }); 

            builder.HasIndex(oiw => new { oiw.OrderItemId, oiw.WarehouseId })
                   .IsUnique();

            builder.HasOne(oiw => oiw.OrderItem)
                   .WithMany(oi => oi.OrderItemWarehouses)
                   .HasForeignKey(oiw => oiw.OrderItemId) // orderItem tablosunda bu ilişkideki fk nedeniyle patlayan kısım burası.
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(oiw => oiw.Warehouse)
                   .WithMany(w => w.OrderItemWarehouses)
                   .HasForeignKey(oiw => oiw.WarehouseId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

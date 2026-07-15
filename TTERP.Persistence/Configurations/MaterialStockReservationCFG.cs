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
    public class MaterialStockReservationCFG : IEntityTypeConfiguration<MaterialStockReservation>
    {
        public void Configure(EntityTypeBuilder<MaterialStockReservation> builder)
        {
            builder.Property(x => x.ConsumedQuantity)
            .IsRequired()
            .HasDefaultValue(0);

            builder.Property(x => x.IsReleased)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.ReservationDate)
                .IsRequired();

            builder.HasOne(x => x.Production)
                .WithMany(x => x.MaterialStockReservations)
                .HasForeignKey(x => x.ProductionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ProductionItem)
                .WithMany(x => x.MaterialStockReservations)
                .HasForeignKey(x => x.ProductionItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Material)
                .WithMany()
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Warehouse)
                .WithMany()
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.ProductionId,
                x.ProductionItemId
            });

            builder.HasIndex(x => new
            {
                x.MaterialId,
                x.WarehouseId,
                x.IsReleased
            });
        }
    }
}

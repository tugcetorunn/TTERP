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
    public class WarehouseCFG : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.Property(w => w.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(w => w.Code)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(w => w.AddressLine)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.HasMany(w => w.SupplyItems)
                   .WithOne(si => si.Warehouse)
                   .HasForeignKey(si => si.WarehouseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(w => w.MaterialWarehouses)
                   .WithOne(mw => mw.Warehouse)
                   .HasForeignKey(mw => mw.WarehouseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(w => w.ProductWarehouses)
                   .WithOne(pw => pw.Warehouse)
                   .HasForeignKey(pw => pw.WarehouseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(w => w.OrderItemWarehouses)
                   .WithOne(oi => oi.Warehouse)
                   .HasForeignKey(oi => oi.WarehouseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(w => w.TargetProductions)
                   .WithOne(oi => oi.TargetWarehouse)
                   .HasForeignKey(oi => oi.TargetWarehouseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.City)
                .WithMany()
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Town)
                .WithMany()
                .HasForeignKey(x => x.TownId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.District)
                .WithMany()
                .HasForeignKey(x => x.DistrictId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Neighborhood)
                .WithMany()
                .HasForeignKey(x => x.NeighborhoodId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

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
    public class SupplierCFG : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.Property(m => m.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(m => m.ContactName)
                   .HasMaxLength(100);

            builder.Property(m => m.ContactEmail)
                   .HasMaxLength(100);

            builder.Property(m => m.ContactPhone)
                   .HasMaxLength(10);

            builder.Property(m => m.AddressLine)
                   .HasMaxLength(300);

            builder.HasMany(m => m.SupplierMaterials)
                   .WithOne(sm => sm.Supplier)
                   .HasForeignKey(sm => sm.SupplierId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.Supplies)
                   .WithOne(s => s.Supplier)
                   .HasForeignKey(s => s.SupplierId)
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

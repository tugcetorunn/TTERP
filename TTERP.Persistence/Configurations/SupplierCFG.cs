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

            builder.Property(m => m.Address)
                   .HasMaxLength(300);

            builder.Property(m => m.City)
                   .HasMaxLength(50);

            builder.Property(m => m.Country)
                   .HasMaxLength(50);

            builder.HasMany(m => m.SupplierMaterials)
                   .WithOne(sm => sm.Supplier)
                   .HasForeignKey(sm => sm.SupplierId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.Supplies)
                   .WithOne(s => s.Supplier)
                   .HasForeignKey(s => s.SupplierId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

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
    public class SupplyCFG : IEntityTypeConfiguration<Supply>
    {
        public void Configure(EntityTypeBuilder<Supply> builder)
        {
            builder.Property(s => s.Quantity)
                   .IsRequired();

            builder.Property(s => s.SupplyDate)
                   .IsRequired()
                   .HasColumnType("date");

            builder.HasOne(s => s.Supplier)
                   .WithMany(su => su.Supplies)
                   .HasForeignKey(s => s.SupplierId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Material)
                   .WithMany(m => m.Supplies)
                   .HasForeignKey(s => s.MaterialId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Employee)
                   .WithMany(e => e.Supplies)
                   .HasForeignKey(s => s.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}

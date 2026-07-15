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
            builder.Property(s => s.SupplyDate)
                   .IsRequired()
                   .HasColumnType("date");

            builder.Property(s => s.DeliveryDate)
                   .IsRequired(false)
                   .HasColumnType("date");

            builder.Property(s => s.DocumentNumber)
                   .IsRequired(false)
                   .HasMaxLength(50);

            builder.Property(s => s.SupplyStatus)
                   .IsRequired()
                   .HasDefaultValue(1);

            builder.HasIndex(x => x.DocumentNumber);

            builder.Property(sm => sm.TotalAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(s => s.Supplier)
                   .WithMany(su => su.Supplies)
                   .HasForeignKey(s => s.SupplierId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Employee)
                   .WithMany(e => e.Supplies)
                   .HasForeignKey(s => s.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.SupplyItems)
                   .WithOne(si => si.Supply)
                   .HasForeignKey(si => si.SupplyId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}

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
    public class PaymentCFG : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.PaymentType)
                   .IsRequired(false)
                   .HasDefaultValue(1); // Nakit

            builder.Property(p => p.PaymentStatus)
                   .IsRequired(false)
                   .HasDefaultValue(1); // Beklemede

            builder.Property(p => p.Note)
                   .IsRequired(false)
                   .HasMaxLength(150);

            builder.Property(x => x.PaymentDate)
                   .IsRequired()
                   .HasColumnType("datetime");

            builder.HasOne(p => p.Order)
                   .WithMany(o => o.Payments)
                   .HasForeignKey(p => p.OrderId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

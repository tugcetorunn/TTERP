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
    public class OrderCFG : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.OrderDate)
                   .IsRequired()
                   .HasColumnType("datetime");

            builder.Property(o => o.CustomerId)
                   .IsRequired();

            builder.Property(o => o.OrderStatus)
                   .IsRequired(false)
                   .HasDefaultValue(1); // Yeni

            builder.Property(o => o.PaymentStatus)
                   .IsRequired(false)
                   .HasDefaultValue(1); // Beklemede

            builder.Property(o => o.ShippingStatus)
                   .IsRequired(false)
                   .HasDefaultValue(1); // Hazýrlanýyor

            builder.Property(o => o.Currency)
                   .IsRequired()
                   .HasDefaultValue(1); // TL

            builder.Property(o => o.TotalAmount)
                   .IsRequired()
                   .HasColumnType("money");

            builder.Property(o => o.Discount)
                   .IsRequired()
                   .HasColumnType("money");

            builder.Property(o => o.FinalAmount)
                   .IsRequired()
                   .HasColumnType("money");

            builder.Property(o => o.ConversationId)
                   .IsRequired(false);

            builder.HasOne(o => o.Customer)
                   .WithMany(c => c.Orders)
                   .HasForeignKey(o => o.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Employee)
                   .WithMany(o => o.Orders)
                   .HasForeignKey(o => o.EmployeeId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(o => o.OrderItems)
                   .WithOne(oi => oi.Order)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
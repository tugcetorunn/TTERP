using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTERP.Domain.Entities;
using Task = TTERP.Domain.Entities.Task;

namespace TTERP.Persistence.Configurations
{
    public class TaskCFG : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Description)
                   .IsRequired(false)
                   .HasMaxLength(250);

            builder.Property(x => x.DueDate)
                   .IsRequired();

            builder.HasOne(x => x.Order)
                   .WithMany(x => x.Tasks)
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Customer)
                   .WithMany(x => x.Tasks)
                   .HasForeignKey(x => x.CustomerId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Product)
                   .WithMany(x => x.Tasks)
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Material)
                   .WithMany(x => x.Tasks)
                   .HasForeignKey(x => x.MaterialId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.AssignedToEmployee)
                   .WithMany(x => x.AssignedTasks)
                   .HasForeignKey(x => x.AssignedToEmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CreatedByEmployee)
                   .WithMany(x => x.CreatedTasks)
                   .HasForeignKey(x => x.CreatedByEmployeeId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

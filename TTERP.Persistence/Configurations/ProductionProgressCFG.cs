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
    public class ProductionProgressCFG : IEntityTypeConfiguration<ProductionProgress>
    {
        public void Configure(EntityTypeBuilder<ProductionProgress> builder)
        {
            builder.Property(pp => pp.Note)
                   .IsRequired(false)
                   .HasMaxLength(200);

            builder.HasOne(x => x.Production)
            .WithMany(x => x.ProductionProgresses)
            .HasForeignKey(x => x.ProductionId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

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
    public class MaterialCFG : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.Property(m => m.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(m => m.Code)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(m => m.Description)
                   .HasMaxLength(300);

            builder.Property(m => m.Unit)
                   .IsRequired()
                   .HasMaxLength(5);

            builder.Property(m => m.CostPrice)
                   .HasColumnType("decimal(18,4)");

            builder.Property(m => m.TaxRate)
                   .HasColumnType("decimal(5,2)");
        }
    }
}

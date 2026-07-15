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
    public class DistrictCFG : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(c => c.Town)
                   .WithMany(c => c.Districts)
                   .HasForeignKey(c => c.TownId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

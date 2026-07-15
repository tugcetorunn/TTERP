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
    public class NeighborhoodCFG : IEntityTypeConfiguration<Neighborhood>
    {
        public void Configure(EntityTypeBuilder<Neighborhood> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(c => c.District)
                   .WithMany(c => c.Neighborhoods)
                   .HasForeignKey(c => c.DistrictId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.PostalCode)
                   .WithMany(c => c.Neighborhoods)
                   .HasForeignKey(c => c.PostalCodeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

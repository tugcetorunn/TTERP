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
    public class CityCFG : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(c => c.Country)
                   .WithMany(c => c.Cities)
                   .HasForeignKey(c => c.CountryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

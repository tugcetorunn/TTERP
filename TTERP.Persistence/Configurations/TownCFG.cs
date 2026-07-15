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
    public class TownCFG : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(c => c.City)
                   .WithMany(c => c.Towns)
                   .HasForeignKey(c => c.CityId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

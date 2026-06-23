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
    public class CustomerCFG : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(x => x.FirstName)
                   .IsRequired(false)
                   .HasMaxLength(50);

            builder.Property(x => x.LastName)
                   .IsRequired(false)
                   .HasMaxLength(50);

            builder.Property(x => x.CompanyName)
                   .IsRequired(false)
                   .HasMaxLength(50);

            builder.Property(x => x.PhoneNumber)
                   .HasMaxLength(10);

            builder.Property(x => x.Address)
                   .IsRequired(false)
                   .HasMaxLength(300);

            builder.Property(x => x.City)
                   .IsRequired(false)
                   .HasMaxLength(50);

            builder.Property(x => x.Country)
                   .IsRequired(false)
                   .HasMaxLength(50);

            builder.HasIndex(x => x.TaxNumber)
                   .IsUnique();

            builder.Property(x => x.TaxNumber)
                   .IsRequired()
                   .HasMaxLength(11);

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(50);
        }
    }
}

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
    public class InvoiceCFG : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasIndex(x => x.InvoiceNumber)
                   .IsUnique();

            builder.Property(x => x.InvoiceNumber)
                .HasMaxLength(16)
                .IsRequired();
        }
    }
}

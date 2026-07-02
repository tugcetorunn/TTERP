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
    public class NotificationCFG : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(80);

            builder.Property(x => x.Message)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(x => x.ActionUrl)
                   .IsRequired(false)
                   .HasMaxLength(150);

            builder.Property(x => x.IsRead)
                   .HasColumnType("bit")
                   .HasDefaultValue(false);
        }
    }
}

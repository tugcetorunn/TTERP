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
    public class ParameterDefinitionCFG : IEntityTypeConfiguration<ParameterDefinition>
    {
        public void Configure(EntityTypeBuilder<ParameterDefinition> builder)
        {
            builder.HasIndex(x => x.ParamType)
                   .IsUnique();

            builder.Property(x => x.ParamType)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Description)
                   .HasMaxLength(200);

            builder.Property(x => x.DataType)
                   .HasMaxLength(15);

            builder.Property(x => x.DefaultValue)
                   .HasMaxLength(100);
        }
    }
}

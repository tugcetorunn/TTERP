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
    public class ParameterValueCFG : IEntityTypeConfiguration<ParameterValue>
    {
        public void Configure(EntityTypeBuilder<ParameterValue> builder)
        {
            builder.HasKey(x => new { x.ParameterDefinitionId, x.ParamValue, x.LanguageId });

            builder.HasOne(x => x.ParameterDefinition)
                .WithMany(x => x.ParameterValues)
                .HasForeignKey(x => x.ParameterDefinitionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.ParamCode)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasIndex(x => new { x.ParameterDefinitionId, x.ParamCode, x.LanguageId })
                   .IsUnique();

            builder.Property(x => x.ParamValue)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .IsRequired(false)
                .HasMaxLength(200);

            builder.Property(x => x.LanguageId)
                .IsRequired();

            builder.ToTable(t => // languageId sadece 1 veya 2 olabilir, bunun için check constraint eklendi;
            {
                t.HasCheckConstraint(
                    "CK_ParameterTranslation_LanguageId",
                    "[LanguageId] IN (1, 2)"
                );
            });
        }
    }
}

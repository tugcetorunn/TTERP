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
    public class WorkflowHistoryCFG : IEntityTypeConfiguration<WorkflowHistory>
    {
        public void Configure(EntityTypeBuilder<WorkflowHistory> builder)
        {
            builder.Property(x => x.Note)
                   .IsRequired(false)
                   .HasMaxLength(150);

        }
    }
}

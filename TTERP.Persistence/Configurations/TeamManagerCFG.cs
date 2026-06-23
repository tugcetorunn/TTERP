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
    public class TeamManagerCFG : IEntityTypeConfiguration<TeamManager>
    {
        public void Configure(EntityTypeBuilder<TeamManager> builder)
        {
            builder.HasKey(x => new { x.TeamId, x.ManagerId});

            builder.HasIndex(x => x.ManagerId)
                   .IsUnique();
        }
    }
}

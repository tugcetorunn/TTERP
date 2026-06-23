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
    public class RoleCFG : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.NameForUI)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.NormalizedName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(x => x.Name)
                   .IsUnique();

            builder.HasIndex(x => x.NameForUI)
                   .IsUnique();

            var admin = new Role
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN",
                NameForUI = "Admin"
            };

            var manager = new Role
            {
                Id = 2,
                Name = "Manager",
                NormalizedName = "MANAGER",
                NameForUI = "Yönetici"
            };

            var user = new Role
            {
                Id = 3,
                Name = "User",
                NormalizedName = "USER",
                NameForUI = "Kullanıcı"
            };

            var auditor = new Role
            {
                Id = 4,
                Name = "Auditor",
                NormalizedName = "AUDITOR",
                NameForUI = "Denetçi"
            };

            admin.SetCreated("System");
            manager.SetCreated("System");
            user.SetCreated("System");
            auditor.SetCreated("System");

            builder.HasData(admin, manager, user, auditor);


        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Entities.Common;

namespace TTERP.Persistence.Configurations
{
    public class EmployeeCFG : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(x => x.FirstName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.LastName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(x => x.PhoneNumber)
                   .IsUnique();

            builder.Property(x => x.PhoneNumber)
                   .HasMaxLength(10);

            builder.HasIndex(x => x.InternalPhone)
                   .IsUnique();

            builder.Property(x => x.InternalPhone)
                   .HasMaxLength(10);

            builder.HasIndex(x => x.NationalId)
                   .IsUnique();

            builder.Property(x => x.NationalId)
                   .IsRequired()
                   .HasMaxLength(11);

            builder.Property(x => x.Address)
                   .HasMaxLength(300);

            builder.Property(x => x.DateOfBirth)
                   .IsRequired()
                   .HasColumnType("date");
                            
            builder.Property(x => x.HireDate)
                   .IsRequired()
                   .HasColumnType("date");

            builder.Property(x => x.LeavingDate)
                   .HasColumnType("date");

            builder.Property(x => x.IsPasswordChanged)
                   .HasColumnType("bit")
                   .HasDefaultValue(false);

            builder.Property(x => x.RegistrationNumber)
                   .IsRequired()
                   .HasMaxLength(6);

            builder.Property(x => x.ImagePath)
                   .HasMaxLength(200);

            builder.Property(x => x.Salary)
                   .IsRequired(false)
                   .HasColumnType("money");

            builder.Property(x => x.RightToAnnualLeave)
                   .IsRequired(false);

            builder.HasOne(x => x.Team)
                   .WithMany(t => t.Members)
                   .HasForeignKey(x => x.TeamId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Title)
                   .WithMany(t => t.Employees)
                   .HasForeignKey(x => x.TitleId)
                   .OnDelete(deleteBehavior: DeleteBehavior.SetNull);

            builder.HasMany(x => x.Notifications)
                   .WithOne(n => n.Employee)
                   .HasForeignKey(n => n.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);

            var admin = new Employee
            {
                Id = 1,
                FirstName = "Ali",
                LastName = "Ergül",
                Email = "ali.ergul@sirket.com.tr",
                NormalizedEmail = "ALI.ERGUL@SIRKET.COM.TR",
                NationalId = "12345678912",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "5551112345",
                RegistrationNumber = 10001,
                HireDate = new DateTime(2015, 1, 1),
                UserName = "aliergul",
                NormalizedUserName = "ALIERGUL",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            admin.SetCreated("System"); 
            admin.PasswordHash = new PasswordHasher<Employee>().HashPassword(admin, "123456");

            builder.HasData(admin);
        }
    }
}

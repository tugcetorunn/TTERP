using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
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

            builder.Property(x => x.Email)
                   .HasMaxLength(100);

            builder.Property(x => x.NormalizedEmail)
                   .HasMaxLength(100);

            builder.HasIndex(x => x.NationalId)
                   .IsUnique();

            builder.Property(x => x.NationalId)
                   .IsRequired()
                   .HasMaxLength(11);

            builder.Property(x => x.AddressLine)
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

            builder.HasIndex(x => x.RegistrationNumber)
                   .IsUnique();

            builder.Property(x => x.RegistrationNumber)
                   .IsRequired()
                   .HasMaxLength(6);

            builder.Property(x => x.ImagePath)
                   .HasMaxLength(200);

            builder.Property(x => x.RightToAnnualLeave)
                   .IsRequired(false);

            builder.HasOne(x => x.Team)
                   .WithMany(t => t.Members)
                   .HasForeignKey(x => x.TeamId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Title)
                   .WithMany(t => t.Employees)
                   .HasForeignKey(x => x.TitleId)
                   .OnDelete(deleteBehavior: DeleteBehavior.Restrict);

            builder.HasMany(x => x.Notifications)
                   .WithOne(n => n.Employee)
                   .HasForeignKey(n => n.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.City)
                .WithMany()
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Town)
                .WithMany()
                .HasForeignKey(x => x.TownId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.District)
                .WithMany()
                .HasForeignKey(x => x.DistrictId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Neighborhood)
                .WithMany()
                .HasForeignKey(x => x.NeighborhoodId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

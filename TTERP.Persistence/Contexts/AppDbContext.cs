using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Entities.Common;

namespace TTERP.Persistence.Contexts
{
    public class AppDbContext : IdentityDbContext<Employee, Role, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ParameterValue> Parameters { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<Domain.Entities.Task> Tasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamManager> TeamManagers { get; set; }
        public DbSet<Title> Titles { get; set; }

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        override protected void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>
                {
                    UserId = 1,
                    RoleId = 1
                }
            );

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity<int>).IsAssignableFrom(entityType.ClrType))
                {
                    builder.Entity(entityType.ClrType)
                        .Property("CreatedDate")
                        .IsRequired()
                        .HasDefaultValueSql("GETUTCDATE()");

                    builder.Entity(entityType.ClrType)
                        .Property("UpdatedDate")
                        .IsRequired(false);

                    builder.Entity(entityType.ClrType)
                        .Property("DeletedDate")
                        .IsRequired(false);

                    builder.Entity(entityType.ClrType)
                        .Property("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50);

                    builder.Entity(entityType.ClrType)
                        .Property("UpdatedBy")
                        .IsRequired(false)
                        .HasMaxLength(50);

                    builder.Entity(entityType.ClrType)
                        .Property("DeletedBy")
                        .IsRequired(false)
                        .HasMaxLength(50);

                    builder.Entity(entityType.ClrType)
                        .Property("IsActive")
                        .IsRequired()
                        .HasDefaultValue(true);

                    builder.Entity(entityType.ClrType)
                        .Property("IsDeleted")
                        .IsRequired()
                        .HasDefaultValue(false);
                }
            }
        }
    }
}

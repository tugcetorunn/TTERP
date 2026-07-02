using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Entities.Common;

namespace TTERP.Persistence.Contexts
{
    public class AppDbContext : IdentityDbContext<Employee, Role, int>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialWarehouse> MaterialWarehouses { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemWarehouse> OrderItemWarehouses { get; set; }
        public DbSet<ParameterDefinition> ParameterDefinitions { get; set; }
        public DbSet<ParameterValue> ParameterValues { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<ProductionItem> ProductionItems { get; set; }
        public DbSet<ProductWarehouse> ProductWarehouses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierMaterial> SupplierMaterials { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<SupplyItem> SupplyItems { get; set; }
        public DbSet<Domain.Entities.Task> Tasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamManager> TeamManagers { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        // handler da tek tek audit property lerine değer atamamak için;
        // IHttpContextAccessor enjekte edip, veritabanına kaydetme (Save) metodunu kendi kurallarımıza göre eziyoruz
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1. İstek atan kullanıcının ID'sini Token'dan okuyoruz
            int? currentUserId = null;
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out int parsedId))
            {
                currentUserId = parsedId;
            }

            // 2. ChangeTracker ile veritabanına gitmek üzere olan tüm nesneleri yakalıyoruz
            // Sadece 'IAuditableEntity' arayüzüne sahip olanları filtreliyoruz
            var entries = ChangeTracker.Entries<IAuditableEntity>();

            foreach (var entry in entries)
            {
                // Eğer nesne YENİ EKLENİYORSA (Insert)
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.SetCreated(currentUserId);
                }
                // Eğer nesne GÜNCELLENİYORSA (Update)
                else if (entry.State == EntityState.Modified)
                {
                    // Mevcut CreatedDate değerinin EF Core tarafından ezilmesini (null olmasını) engelliyoruz
                    entry.Property(p => p.CreatedDate).IsModified = false;
                    entry.Property(p => p.CreatedBy).IsModified = false;

                    entry.Entity.SetUpdated(currentUserId);
                }
            }

            // 3. Değişiklikleri (bizim eklediğimiz audit verileriyle birlikte) veritabanına gönderiyoruz
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            // decimal tipli propertyler için genel configuration (cfg içinde ezilebilir)
            configurationBuilder.Properties<decimal>()
                                .HaveColumnType("decimal(18,2)");
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

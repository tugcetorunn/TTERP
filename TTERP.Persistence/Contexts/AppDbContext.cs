using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public AppDbContext()
        {
            
        }
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
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Neighborhood> Neighborhoods { get; set; }
        public DbSet<PostalCode> PostalCodes { get; set; }
        public DbSet<WorkflowHistory> WorkflowHistories { get; set; }
        public DbSet<WorkflowTransition> WorkflowTransitions { get; set; }
        public DbSet<ProductionProgress> ProductionProgresses { get; set; }
        public DbSet<MaterialStockReservation> MaterialStockReservations { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

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

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var entityBuilder = builder.Entity(entityType.ClrType);

                if (entityType.FindProperty("CreatedDate") is not null)
                {
                    entityBuilder.Property("CreatedDate")
                        .IsRequired()
                        .HasDefaultValueSql("GETUTCDATE()");
                }

                if (entityType.FindProperty("UpdatedDate") is not null)
                {
                    entityBuilder.Property("UpdatedDate")
                        .IsRequired(false);
                }

                if (entityType.FindProperty("DeletedDate") is not null)
                {
                    entityBuilder.Property("DeletedDate")
                        .IsRequired(false);
                }

                if (entityType.FindProperty("CreatedBy") is not null)
                {
                    entityBuilder.Property("CreatedBy")
                        .IsRequired()
                        .HasDefaultValue(1);
                }

                if (entityType.FindProperty("UpdatedBy") is not null)
                {
                    entityBuilder.Property("UpdatedBy")
                        .IsRequired(false);
                }

                if (entityType.FindProperty("DeletedBy") is not null)
                {
                    entityBuilder.Property("DeletedBy")
                        .IsRequired(false);
                }

                if (entityType.FindProperty("IsActive") is not null)
                {
                    entityBuilder.Property("IsActive")
                        .IsRequired()
                        .HasDefaultValue(true);
                }

                if (entityType.FindProperty("IsDeleted") is not null)
                {
                    entityBuilder.Property("IsDeleted")
                        .IsRequired()
                        .HasDefaultValue(false);
                }
            }
        }
    }
}

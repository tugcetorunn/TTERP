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
    public class WorkflowTransitionCFG : IEntityTypeConfiguration<WorkflowTransition>
    {
        public void Configure(EntityTypeBuilder<WorkflowTransition> builder)
        {
            builder.Property(m => m.RequiredRole)
                   .IsRequired(false)
                   .HasMaxLength(100);

            builder.HasIndex(x => new
            {
                x.WorkflowType,
                x.FromStatusCode,
                x.ToStatusCode
            }).IsUnique();

            var seedDate = new DateTime(2026, 7, 13);

            builder.HasData(
            // SUPPLY
            CreateSeed(1, 1, 1, 2, false, false, 1, seedDate),
            CreateSeed(2, 1, 2, 3, false, false, 2, seedDate),
            CreateSeed(3, 1, 3, 4, true, true, 3, seedDate),
            CreateSeed(4, 1, 1, 5, true, false, 4, seedDate),
            CreateSeed(5, 1, 2, 5, true, false, 5, seedDate),
            CreateSeed(6, 1, 3, 5, true, false, 6, seedDate),

            // PRODUCTION
            CreateSeed(7, 2, 1, 2, true, false, 1, seedDate),
            CreateSeed(8, 2, 2, 3, false, false, 2, seedDate),
            CreateSeed(9, 2, 3, 2, false, false, 3, seedDate),
            CreateSeed(10, 2, 2, 4, true, true, 4, seedDate),
            CreateSeed(11, 2, 1, 5, true, false, 5, seedDate),
            CreateSeed(12, 2, 2, 5, true, false, 6, seedDate),
            CreateSeed(13, 2, 3, 5, true, false, 7, seedDate),

            // SALES
            CreateSeed(14, 3, 1, 2, false, false, 1, seedDate),
            CreateSeed(15, 3, 2, 3, true, false, 2, seedDate),
            CreateSeed(16, 3, 2, 4, true, false, 3, seedDate),
            CreateSeed(17, 3, 3, 5, true, true, 4, seedDate),
            CreateSeed(18, 3, 1, 6, true, false, 5, seedDate),
            CreateSeed(19, 3, 2, 6, true, false, 6, seedDate),
            CreateSeed(20, 3, 3, 6, true, false, 7, seedDate)
        );
        }

        private static object CreateSeed(
            int id,
            int workflowType,
            int fromStatusCode,
            int toStatusCode,
            bool requiresConfirmation,
            bool createsStockMovement,
            int displayOrder,
            DateTime seedDate,
            string? requiredRole = null)
        {
            return new
            {
                Id = id,
                WorkflowType = workflowType,
                FromStatusCode = fromStatusCode,
                ToStatusCode = toStatusCode,

                // ActionCode aynı modülün hedef statü kodudur.
                ActionCode = toStatusCode,

                RequiredRole = requiredRole,
                RequiresConfirmation = requiresConfirmation,
                CreatesStockMovement = createsStockMovement,
                DisplayOrder = displayOrder,

                CreatedDate = seedDate,
                UpdatedDate = (DateTime?)null,
                DeletedDate = (DateTime?)null,
                CreatedBy = (int?)null,
                UpdatedBy = (int?)null,
                DeletedBy = (int?)null,
                IsActive = true,
                IsDeleted = false,
                LanguageSupportId = (int?)1
            };
        }
    }
}

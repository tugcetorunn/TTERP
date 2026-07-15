using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Persistence.Contexts;
using TTERP.Persistence.Repositories.Abstract;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class MaterialStockReservationRepository : BaseRepository<MaterialStockReservation>, IMaterialStockReservationRepository
    {
        public MaterialStockReservationRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<double> GetActiveReservedQuantityAsync(int warehouseId, int materialId, CancellationToken cancellationToken = default)
        {
            return await context.MaterialStockReservations
                                .Where(x =>
                                    x.WarehouseId == warehouseId &&
                                    x.MaterialId == materialId &&
                                    !x.IsReleased &&
                                    x.IsActive &&
                                    !x.IsDeleted)
                                .SumAsync(x => x.ReservedQuantity - x.ConsumedQuantity, cancellationToken);
        }

        public async Task<List<MaterialStockReservation>> GetByProductionIdAsync(int productionId, CancellationToken cancellationToken = default)
        {
            return await context.MaterialStockReservations
                                .Include(x => x.Material)
                                .Include(x => x.Warehouse)
                                .Include(x => x.ProductionItem)
                                .Where(x =>
                                    x.ProductionId == productionId &&
                                    !x.IsDeleted)
                                .ToListAsync(cancellationToken);
        }
    }
}

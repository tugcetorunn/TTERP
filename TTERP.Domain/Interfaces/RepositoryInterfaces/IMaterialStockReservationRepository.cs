using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Domain.Interfaces.RepositoryInterfaces
{
    public interface IMaterialStockReservationRepository : IBaseRepository<MaterialStockReservation>
    {
        Task<double> GetActiveReservedQuantityAsync(int warehouseId, int materialId, CancellationToken cancellationToken = default);
        Task<List<MaterialStockReservation>> GetByProductionIdAsync(int productionId, CancellationToken cancellationToken = default);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace TTERP.Domain.Interfaces
{
    public interface IMaterialWarehouseRepository : IBaseRepository<MaterialWarehouse>
    {
        Task DecreaseStockAsync(int warehouseId, int materialId, double quantity, int? reason, CancellationToken cancellationToken = default);
        Task IncreaseStockAsync(int warehouseId, int materialId, double quantity, int? reason, CancellationToken cancellationToken = default);
        Task<MaterialWarehouse?> GetByMaterialAndWarehouseAsync(int materialId, int warehouseId, CancellationToken cancellationToken = default);
        Task<List<MaterialsStockModel>> GetMaterialsStockAsync(int? materialId = null, int? warehouseId = null, CancellationToken cancellationToken = default);
        Task<double> GetTotalStockAsync(int warehouseId, int materialId, CancellationToken cancellationToken = default);
        Task<MaterialWarehouse?> GetLastMovementAsync(int materialId, int warehouseId, CancellationToken cancellationToken = default);
        Task<MaterialStockSummaryDTO> GetStockSummaryAsync(int sourceWarehouseId, int materialId, CancellationToken cancellationToken);
    }
}

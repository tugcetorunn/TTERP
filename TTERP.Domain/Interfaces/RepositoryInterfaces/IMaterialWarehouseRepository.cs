using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TTERP.Domain.Interfaces
{
    public interface IMaterialWarehouseRepository : IBaseRepository<MaterialWarehouse>
    {
        Task DecreaseStockAsync(int warehouseId, int materialId, double quantity, int? reason, CancellationToken cancellationToken = default);
        Task IncreaseStockAsync(int warehouseId, int materialId, double quantity, int? reason, CancellationToken cancellationToken = default);
        Task<MaterialWarehouse?> GetByMaterialAndWarehouseAsync(int materialId, int warehouseId, CancellationToken cancellationToken = default);
    }
}

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
    public interface IProductWarehouseRepository : IBaseRepository<ProductWarehouse>
    {
        Task DecreaseStockAsync(int warehouseId, int productId, double quantity, int? reason, CancellationToken cancellationToken = default);
        Task IncreaseStockAsync(int warehouseId, int productId, double quantity, int? reason, CancellationToken cancellationToken = default);
        Task<List<ProductsStockModel>> GetProductsStockAsync(int? productId = null, int? warehouseId = null, CancellationToken cancellationToken = default);
    }
}

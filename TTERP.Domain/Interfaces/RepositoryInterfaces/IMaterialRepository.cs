using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Domain.Interfaces
{
    public interface IMaterialRepository : IBaseRepository<Material>
    {
        Task<List<int>?> GetMaterialIdsAsync(bool isActive = true, bool isDeleted = false);
        Task<Dictionary<int, int>> GetSupplierCountOfMaterialsAsync(List<int>? materialIds);
        Task<Dictionary<int, int>> GetWarehouseCountOfMaterialsAsync(List<int>? materialIds);
    }
}

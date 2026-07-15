using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Persistence.Contexts;
using TTERP.Persistence.Repositories.Abstract;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class MaterialRepository : BaseRepository<Material>, IMaterialRepository
    {
        public MaterialRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<List<int>?> GetMaterialIdsAsync(bool isActive = true, bool isDeleted = false)
        {
            return await context.Materials
                                .Where(m => m.IsActive == isActive && m.IsDeleted == isDeleted)
                                .Select(m => m.Id)
                                .ToListAsync();
        }

        public async Task<Dictionary<int, int>> GetSupplierCountOfMaterialsAsync(List<int>? materialIds)
        {
            if (materialIds == null || !materialIds.Any())
                return new Dictionary<int, int>();

            return await context.SupplierMaterials
                .Where(x => materialIds.Contains(x.MaterialId))
                .GroupBy(x => x.MaterialId)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Count());
        }

        public async Task<Dictionary<int, int>> GetWarehouseCountOfMaterialsAsync(List<int>? materialIds)
        {
            if (materialIds == null || !materialIds.Any())
                return new Dictionary<int, int>();

            return await context.MaterialWarehouses
                .Where(x => materialIds.Contains(x.MaterialId))
                .GroupBy(x => x.MaterialId)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Count());
        }
    }
}

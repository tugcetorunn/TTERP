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
using Task = System.Threading.Tasks.Task;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class MaterialWarehouseRepository : BaseRepository<MaterialWarehouse>, IMaterialWarehouseRepository
    {
        public MaterialWarehouseRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task DecreaseStockAsync(int warehouseId, int materialId, double quantity, int? reason, CancellationToken cancellationToken = default)
        {
            var materialWarehouse = await context.MaterialWarehouses
                .FirstOrDefaultAsync(mw => mw.WarehouseId == warehouseId && mw.MaterialId == materialId, cancellationToken);

            if (materialWarehouse == null)
                throw new InvalidOperationException($"Hata: Seçilen depoda (ID: {warehouseId}) bu hammaddeye (ID: {materialId}) ait kayıt bulunamadı.");

            if (materialWarehouse.Quantity < quantity)
                throw new InvalidOperationException($"Hata: Seçilen depoda (ID: {warehouseId}) bu hammaddeye (ID: {materialId}) ait yeterli stok bulunmamaktadır. Mevcut stok: {materialWarehouse.Quantity}, Talep edilen miktar: {quantity}.");

            materialWarehouse.ReasonForEntryOrExit = reason;
            materialWarehouse.Quantity -= quantity;

            var material = await context.Materials.FirstOrDefaultAsync(m => m.Id == materialWarehouse.MaterialId, cancellationToken);
            if (material != null)
            {
                material.StockQuantity -= quantity;

            }
        }

        public async Task IncreaseStockAsync(int warehouseId, int materialId, double quantity, int? reason, CancellationToken cancellationToken = default)
        {
            var materialWarehouse = await context.MaterialWarehouses
                .FirstOrDefaultAsync(mw => mw.WarehouseId == warehouseId && mw.MaterialId == materialId, cancellationToken);

            if (materialWarehouse == null)
            {
                materialWarehouse = new MaterialWarehouse
                {
                    WarehouseId = warehouseId,
                    MaterialId = materialId,
                    Quantity = quantity,
                    ReasonForEntryOrExit = reason
                };

                await context.MaterialWarehouses.AddAsync(materialWarehouse, cancellationToken);
            }
            else
            {
                materialWarehouse.Quantity += quantity;
                materialWarehouse.ReasonForEntryOrExit = reason;
            }

            var material = await context.Materials.FirstOrDefaultAsync(m => m.Id == materialWarehouse.MaterialId, cancellationToken);
            if (material != null)
            {
                material.StockQuantity += quantity;

            }
        }
        public async Task<MaterialWarehouse?> GetByMaterialAndWarehouseAsync(int materialId, int warehouseId, CancellationToken cancellationToken = default)
        {
            return await context.MaterialWarehouses
                .FirstOrDefaultAsync(mw => mw.MaterialId == materialId && mw.WarehouseId == warehouseId && !mw.IsDeleted && mw.IsActive, cancellationToken);
        }
    }
}

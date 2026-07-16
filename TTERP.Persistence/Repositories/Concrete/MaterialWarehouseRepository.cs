using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Domain.Models;
using TTERP.Persistence.Contexts;
using TTERP.Persistence.Repositories.Abstract;
using Task = System.Threading.Tasks.Task;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class MaterialWarehouseRepository : BaseRepository<MaterialWarehouse>, IMaterialWarehouseRepository
    {
        private readonly IMaterialStockReservationRepository _reservationRepository;
        private readonly IParameterValueRepository _parameterValueRepository;

        public MaterialWarehouseRepository(AppDbContext _context, IMaterialStockReservationRepository reservationRepository, IParameterValueRepository parameterValueRepository) : base(_context)
        {
            _reservationRepository = reservationRepository;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task DecreaseStockAsync(int warehouseId, int materialId, double quantity, int? reason, CancellationToken cancellationToken = default)
        {
            //var materialWarehouse = await context.MaterialWarehouses
            //    .FirstOrDefaultAsync(mw => mw.WarehouseId == warehouseId && mw.MaterialId == materialId, cancellationToken);

            //if (materialWarehouse == null)
            //    throw new InvalidOperationException($"Hata: Seçilen depoda (ID: {warehouseId}) bu hammaddeye (ID: {materialId}) ait kayıt bulunamadı.");

            //if (materialWarehouse.Quantity < quantity)
            //    throw new InvalidOperationException($"Hata: Seçilen depoda (ID: {warehouseId}) bu hammaddeye (ID: {materialId}) ait yeterli stok bulunmamaktadır. Mevcut stok: {materialWarehouse.Quantity}, Talep edilen miktar: {quantity}.");

            //materialWarehouse.ReasonForEntryOrExit = reason;
            //materialWarehouse.Quantity -= quantity;

            //var material = await context.Materials.FirstOrDefaultAsync(m => m.Id == materialWarehouse.MaterialId, cancellationToken);
            //if (material != null)
            //{
            //    material.StockQuantity -= quantity;

            //}

            if (quantity <= 0)
            {
                throw new ArgumentException(
                    "Stok çıkış miktarı sıfırdan büyük olmalıdır.",
                    nameof(quantity));
            }

            var currentStock = await context.MaterialWarehouses
                .Where(x =>
                    x.WarehouseId == warehouseId &&
                    x.MaterialId == materialId &&
                    x.IsActive &&
                    !x.IsDeleted)
                .SumAsync(
                    x => (double?)x.Quantity,
                    cancellationToken) ?? 0;

            if (currentStock < quantity)
            {
                throw new InvalidOperationException(
                    $"Seçilen depoda yeterli hammadde stoğu bulunmamaktadır. " +
                    $"Depo ID: {warehouseId}, Malzeme ID: {materialId}, " +
                    $"Mevcut stok: {currentStock}, Talep edilen: {quantity}.");
            }

            var movement = new MaterialWarehouse
            {
                WarehouseId = warehouseId,
                MaterialId = materialId,
                Quantity = -quantity,
                ReasonForEntryOrExit = reason
            };

            await context.MaterialWarehouses.AddAsync(
                movement,
                cancellationToken);

            var material = await context.Materials
                .FirstOrDefaultAsync(
                    x => x.Id == materialId,
                    cancellationToken);

            if (material != null)
            {
                material.StockQuantity -= quantity;
            }
        }

        public async Task IncreaseStockAsync(int warehouseId, int materialId, double quantity, int? reason, CancellationToken cancellationToken = default)
        {
            //var materialWarehouse = await context.MaterialWarehouses
            //    .FirstOrDefaultAsync(mw => mw.WarehouseId == warehouseId && mw.MaterialId == materialId, cancellationToken);

            //if (materialWarehouse == null)
            //{
            //    materialWarehouse = new MaterialWarehouse
            //    {
            //        WarehouseId = warehouseId,
            //        MaterialId = materialId,
            //        Quantity = quantity,
            //        ReasonForEntryOrExit = reason
            //    };

            //    await context.MaterialWarehouses.AddAsync(materialWarehouse, cancellationToken);
            //}
            //else
            //{
            //    materialWarehouse.Quantity += quantity;
            //    materialWarehouse.ReasonForEntryOrExit = reason;
            //}

            //var material = await context.Materials.FirstOrDefaultAsync(m => m.Id == materialWarehouse.MaterialId, cancellationToken);
            //if (material != null)
            //{
            //    material.StockQuantity += quantity;

            //}

            if (quantity <= 0)
            {
                throw new ArgumentException(
                    "Stok giriş miktarı sıfırdan büyük olmalıdır.",
                    nameof(quantity));
            }

            var movement = new MaterialWarehouse
            {
                WarehouseId = warehouseId,
                MaterialId = materialId,
                Quantity = quantity,
                ReasonForEntryOrExit = reason
            };

            await context.MaterialWarehouses.AddAsync(
                movement,
                cancellationToken);

            var material = await context.Materials
                .FirstOrDefaultAsync(
                    x => x.Id == materialId,
                    cancellationToken);

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

        public async Task<List<MaterialsStockModel>> GetMaterialsStockAsync(int? materialId = null, int? warehouseId = null, CancellationToken cancellationToken = default)
        {
            var query = context.MaterialWarehouses.AsNoTracking().Where(x => !x.IsDeleted && x.IsActive);

            if (materialId.HasValue)
            {
                query = query.Where(x => x.MaterialId == materialId.Value);
            }

            if (warehouseId.HasValue)
            {
                query = query.Where(x => x.WarehouseId == warehouseId.Value);
            }

            var stocks = await query
                .GroupBy(x => new
                {
                    x.MaterialId,
                    MaterialName = x.Material!.Name,
                    MaterialCode = x.Material.Code,
                    MaterialUnit = x.Material.Unit,

                    x.WarehouseId,
                    WarehouseName = x.Warehouse!.Name,
                    WarehouseCode = x.Warehouse.Code
                })
                .Select(group => new MaterialsStockModel
                {
                    MaterialId = group.Key.MaterialId,
                    MaterialName = group.Key.MaterialName,
                    MaterialCode = group.Key.MaterialCode,
                    MaterialUnit = (int)group.Key.MaterialUnit!,

                    WarehouseId = group.Key.WarehouseId,
                    WarehouseName = group.Key.WarehouseName,
                    WarehouseCode = group.Key.WarehouseCode,

                    TotalQuantity = group.Sum(x => x.Quantity),

                    IsActive = true,
                    IsDeleted = false
                })
                .OrderBy(x => x.MaterialName)
                .ThenBy(x => x.WarehouseName)
                .ToListAsync(cancellationToken);

            var unitValues = await _parameterValueRepository
                .GetParamValuesByParamTypeAsync(
                    "MaterialUnit",
                    1,
                    cancellationToken);

            var unitDictionary = unitValues
                .Where(x => x != null)
                .GroupBy(x => x!.ParamCode)
                .ToDictionary(
                    group => group.Key,
                    group => group.First()!.ParamValue);

            var reservationQuery = context.MaterialStockReservations
                .AsNoTracking()
                .Where(x =>
                    !x.IsReleased &&
                    x.IsActive &&
                    !x.IsDeleted);

            if (materialId.HasValue)
            {
                reservationQuery = reservationQuery.Where(
                    x => x.MaterialId == materialId.Value);
            }

            if (warehouseId.HasValue)
            {
                reservationQuery = reservationQuery.Where(
                    x => x.WarehouseId == warehouseId.Value);
            }

            var reservations = await reservationQuery
                .GroupBy(x => new
                {
                    x.MaterialId,
                    x.WarehouseId
                })
                .Select(group => new
                {
                    group.Key.MaterialId,
                    group.Key.WarehouseId,

                    ReservedQuantity = group.Sum(x =>
                        x.ReservedQuantity - x.ConsumedQuantity > 0
                            ? x.ReservedQuantity - x.ConsumedQuantity
                            : 0)
                })
                .ToListAsync(cancellationToken);

            var reservationDictionary = reservations.ToDictionary(
                x => (x.MaterialId, x.WarehouseId),
                x => x.ReservedQuantity);

            foreach (var stock in stocks)
            {
                stock.MaterialUnitName =
                    unitDictionary.GetValueOrDefault(stock.MaterialUnit)!;

                reservationDictionary.TryGetValue(
                    (stock.MaterialId, stock.WarehouseId),
                    out var reservedQuantity);

                stock.ReservedQuantity = Math.Max(0, reservedQuantity);

                stock.AvailableQuantity =
                    stock.TotalQuantity - stock.ReservedQuantity;
            }

            return stocks;
        }

        public async Task<MaterialStockSummaryDTO> GetStockSummaryAsync(int sourceWarehouseId, int materialId, CancellationToken cancellationToken)
        {
            var physicalStock = await context.MaterialWarehouses
                                           .Where(x =>
                                               x.WarehouseId == sourceWarehouseId &&
                                               x.MaterialId == materialId &&
                                               x.IsActive &&
                                               !x.IsDeleted)
                                           .SumAsync(x => x.Quantity, cancellationToken);

                                                var reservedStock =
                                                    await _reservationRepository.GetActiveReservedQuantityAsync(
                                                        sourceWarehouseId,
                                                        materialId,
                                                        cancellationToken);

                                                return new MaterialStockSummaryDTO
                                                {
                                                    PhysicalStock = physicalStock,
                                                    ReservedStock = reservedStock
                                                };
        }

        public async Task<double> GetTotalStockAsync(int warehouseId, int materialId, CancellationToken cancellationToken = default)
        {
            return await context.MaterialWarehouses
                                .Where(x =>
                                    x.WarehouseId == warehouseId &&
                                    x.MaterialId == materialId &&
                                    x.IsActive &&
                                    !x.IsDeleted)
                                .SumAsync(
                                    x => (double?)x.Quantity,
                                    cancellationToken) ?? 0;
        }

        public async Task<MaterialWarehouse?> GetLastMovementAsync(int materialId, int warehouseId, CancellationToken cancellationToken = default)
        {
            return await context.MaterialWarehouses
                                .AsNoTracking()
                                .Where(x =>
                                    x.MaterialId == materialId &&
                                    x.WarehouseId == warehouseId &&
                                    x.IsActive &&
                                    !x.IsDeleted)
                                .OrderByDescending(x => x.CreatedDate)
                                .ThenByDescending(x => x.Id)
                                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

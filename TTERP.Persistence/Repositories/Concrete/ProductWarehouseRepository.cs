using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Models;
using TTERP.Persistence.Contexts;
using TTERP.Persistence.Repositories.Abstract;
using Task = System.Threading.Tasks.Task;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class ProductWarehouseRepository : BaseRepository<ProductWarehouse>, IProductWarehouseRepository
    {
        public ProductWarehouseRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task DecreaseStockAsync(int warehouseId, int productId, double quantity, int? reason, CancellationToken cancellationToken = default)
        {
            var product = await context.Products
                .FirstOrDefaultAsync(
                    x => x.Id == productId,
                    cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException(
                    $"Stok miktarı güncellenecek ürün bulunamadı. Ürün ID: {productId}");
            }

            if (quantity <= 0)
            {
                throw new ArgumentException(
                    "Ürün stok çıkış miktarı sıfırdan büyük olmalıdır.",
                    nameof(quantity));
            }

            var currentStock = await context.ProductWarehouses
                .Where(x =>
                    x.WarehouseId == warehouseId &&
                    x.ProductId == productId &&
                    x.IsActive &&
                    !x.IsDeleted)
                .SumAsync(
                    x => (double?)x.Quantity,
                    cancellationToken) ?? 0;

            if (currentStock < quantity)
            {
                throw new InvalidOperationException(
                    $"Seçilen depoda yeterli ürün stoğu bulunmamaktadır. " +
                    $"Depo ID: {warehouseId}, Ürün ID: {productId}, " +
                    $"Mevcut stok: {currentStock}, Talep edilen: {quantity}.");
            }

            var movement = new ProductWarehouse
            {
                WarehouseId = warehouseId,
                ProductId = productId,
                Quantity = -quantity,
                ReasonForEntryOrExit = reason
            };

            await context.ProductWarehouses.AddAsync(
                movement,
                cancellationToken);

        }


        public async Task IncreaseStockAsync(int warehouseId, int productId, double quantity, int? reason, CancellationToken cancellationToken = default)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException(
                    "Ürün stok giriş miktarı sıfırdan büyük olmalıdır.",
                    nameof(quantity));
            }

            var movement = new ProductWarehouse
            {
                WarehouseId = warehouseId,
                ProductId = productId,
                Quantity = quantity,
                ReasonForEntryOrExit = reason
            };

            await context.ProductWarehouses.AddAsync(
                movement,
                cancellationToken);

            var product = await context.Products
                .FirstOrDefaultAsync(
                    x => x.Id == productId,
                    cancellationToken);

            if (product != null)
            {
                product.StockQuantity += quantity;
            }
        }
        public async Task<List<ProductsStockModel>> GetProductsStockAsync(int? productId = null, int? warehouseId = null, CancellationToken cancellationToken = default)
        {
            var query = context.ProductWarehouses.AsNoTracking()
                                      .Where(x => !x.IsDeleted && x.IsActive);

            if (productId.HasValue)
            {
                query = query.Where(x => x.ProductId == productId.Value);
            }

            if (warehouseId.HasValue)
            {
                query = query.Where(x => x.WarehouseId == warehouseId.Value);
            }

            return await query.GroupBy(x => new
            {
                x.ProductId,
                ProductCode = x.Product!.Code,
                ProductName = x.Product!.Name,

                x.WarehouseId,
                WarehouseName = x.Warehouse!.Name,
                WarehouseCode = x.Warehouse.Code
            })
                              .Select(group => new ProductsStockModel
                              {
                                  ProductId = group.Key.ProductId,
                                  ProductName = group.Key.ProductName,
                                  ProductCode = group.Key.ProductCode,

                                  WarehouseId = group.Key.WarehouseId,
                                  WarehouseName = group.Key.WarehouseName,
                                  WarehouseCode = group.Key.WarehouseCode,

                                  TotalQuantity = group.Sum(x => x.Quantity),

                                  IsActive = true,
                                  IsDeleted = false
                              })
                              .OrderBy(x => x.ProductName)
                              .ThenBy(x => x.WarehouseName)
                              .ToListAsync(cancellationToken);
        }
    }
}

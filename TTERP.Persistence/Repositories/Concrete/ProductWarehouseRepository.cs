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
    public class ProductWarehouseRepository : BaseRepository<ProductWarehouse>, IProductWarehouseRepository
    {
        public ProductWarehouseRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task DecreaseStockAsync(int warehouseId, int productId, double quantity, int? reason, CancellationToken cancellationToken = default)
        {
            var productWarehouse = await context.Set<ProductWarehouse>()
                                                .FirstOrDefaultAsync(pw => pw.WarehouseId == warehouseId && pw.ProductId == productId, cancellationToken);

            if (productWarehouse == null)
                throw new InvalidOperationException($"Sipariş iptal edildi: Seçilen depoda (ID: {warehouseId}) bu ürüne (ID: {productId}) ait stok kaydı bulunamadı.");

            if (productWarehouse.Quantity < quantity)
                throw new InvalidOperationException($"Sipariş iptal edildi: Seçilen depoda (ID: {warehouseId}) bu ürüne (ID: {productId}) ait stok yetersiz. İstenen miktar: {quantity}, depodaki miktar: {productWarehouse.Quantity}");

            productWarehouse.ReasonForEntryOrExit = reason;
            productWarehouse.Quantity -= quantity;

            var product = await context.Set<Product>().FirstOrDefaultAsync(p => p.Id == productWarehouse.ProductId, cancellationToken);
            if(product != null)
            {
                product.StockQuantity -= quantity;
            }
        }

        public async Task IncreaseStockAsync(int warehouseId, int productId, double quantity, int? reason, CancellationToken cancellationToken = default)
        {
            var productWarehouse = await context.Set<ProductWarehouse>()
                                                .FirstOrDefaultAsync(pw => pw.WarehouseId == warehouseId && pw.ProductId == productId, cancellationToken);

            if (productWarehouse == null)
            {
                productWarehouse = new ProductWarehouse
                {
                    WarehouseId = warehouseId,
                    ProductId = productId,
                    Quantity = quantity,
                    ReasonForEntryOrExit = reason
                };
                await context.Set<ProductWarehouse>().AddAsync(productWarehouse, cancellationToken);
            }
            else
            {
                productWarehouse.ReasonForEntryOrExit = reason;
                productWarehouse.Quantity += quantity;
            }

            var product = await context.Set<Product>().FirstOrDefaultAsync(p => p.Id == productWarehouse.ProductId, cancellationToken);
            if (product != null)
            {
                product.StockQuantity += quantity;
            }
        }
    }
}

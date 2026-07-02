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
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<int> GetCurrencyByProductId(int productId, CancellationToken cancellationToken = default)
        {
            return await context.Products
                                .Where(p => p.Id == productId)
                                .Select(p => p.Currency)
                                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<decimal> GetTaxRateByProductId(int productId, CancellationToken cancellationToken = default)
        {
            return await context.Products
                                .Where(p => p.Id == productId)
                                .Select(p => p.TaxRate)
                                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

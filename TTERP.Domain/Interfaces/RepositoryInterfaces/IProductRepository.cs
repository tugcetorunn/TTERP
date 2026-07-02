using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Domain.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<decimal> GetTaxRateByProductId(int productId, CancellationToken cancellationToken = default);
        Task<int> GetCurrencyByProductId(int productId, CancellationToken cancellationToken = default);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Domain.Interfaces.ServiceInterfaces
{
    public interface ICurrencyService
    {
        Task<decimal> GetExchangeRateAsync(int fromCurrencyId, int toCurrencyId, DateTime date, CancellationToken cancellationToken = default);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ExchangeRates;

namespace TTERP.Application.Interfaces
{
    public interface IExchangeRateStore
    {
        IReadOnlyCollection<ExchangeRateDTO> GetAll();
        ExchangeRateDTO? GetBySymbol(string symbol);
        ExchangeRateDTO AddOrUpdate(string symbol, decimal price, DateTimeOffset updatedAt);
        IReadOnlyCollection<ExchangeRateDTO> CalculateMissingRates(DateTimeOffset updatedAt);
    }
}

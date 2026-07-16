using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ExchangeRates;

namespace TTERP.Application.Interfaces
{
    public interface IExchangeRatePublisher
    {
        Task PublishAsync(ExchangeRateDTO exchangeRate, CancellationToken cancellationToken = default);
    }
}

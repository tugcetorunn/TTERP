using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ExchangeRates.Queries;
using TTERP.Application.Interfaces;
using TTERP.Application.Models.DTOs.ExchangeRates;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ExchangeRates.Handlers
{
    public sealed class GetCurrentExchangeRatesQueryHandler : IRequestHandler<GetCurrentExchangeRatesQuery, Response<IReadOnlyCollection<ExchangeRateDTO>>>
    {
        private readonly IExchangeRateStore _exchangeRateStore;

        public GetCurrentExchangeRatesQueryHandler(IExchangeRateStore exchangeRateStore)
        {
            _exchangeRateStore = exchangeRateStore;
        }

        public Task<Response<IReadOnlyCollection<ExchangeRateDTO>>> Handle(GetCurrentExchangeRatesQuery request, CancellationToken cancellationToken)
        {
            var rates = _exchangeRateStore.GetAll();

            return Task.FromResult(Response<IReadOnlyCollection<ExchangeRateDTO>>.Success(rates));
        }
    }
}

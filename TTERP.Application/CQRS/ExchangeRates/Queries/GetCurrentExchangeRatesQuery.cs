using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ExchangeRates;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ExchangeRates.Queries
{
    public sealed record GetCurrentExchangeRatesQuery : IRequest<Response<IReadOnlyCollection<ExchangeRateDTO>>>
    {
    }
}

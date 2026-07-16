using Microsoft.AspNetCore.SignalR;
using TTERP.Application.Interfaces;
using TTERP.Application.Models.DTOs.ExchangeRates;
using TTERP.WebApi.Hubs;

namespace TTERP.WebApi.Services
{
    public sealed class SignalRExchangeRatePublisher
    : IExchangeRatePublisher
    {
        private readonly IHubContext<ExchangeRateHub> _hubContext;

        public SignalRExchangeRatePublisher(
            IHubContext<ExchangeRateHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task PublishAsync(
            ExchangeRateDTO exchangeRate,
            CancellationToken cancellationToken = default)
        {
            return _hubContext.Clients.All.SendAsync(
                "ExchangeRateUpdated",
                exchangeRate,
                cancellationToken);
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using TTERP.Application.Interfaces;

namespace TTERP.WebApi.Hubs
{
    public sealed class ExchangeRateHub : Hub
    {
        private readonly IExchangeRateStore _exchangeRateStore;

        public ExchangeRateHub(IExchangeRateStore exchangeRateStore)
        {
            _exchangeRateStore = exchangeRateStore;
        }

        public override async Task OnConnectedAsync()
        {
            var currentRates = _exchangeRateStore.GetAll();

            await Clients.Caller.SendAsync(
                "ExchangeRatesSnapshot",
                currentRates);

            await base.OnConnectedAsync();
        }
    }
}

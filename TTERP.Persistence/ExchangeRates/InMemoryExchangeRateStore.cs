using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Interfaces;
using TTERP.Application.Models.DTOs.ExchangeRates;

namespace TTERP.Persistence.ExchangeRates
{
    public sealed class InMemoryExchangeRateStore : IExchangeRateStore
    {
        private readonly ConcurrentDictionary<string, ExchangeRateDTO> _rates =
            new(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<ExchangeRateDTO> GetAll()
        {
            return _rates.Values
                .OrderBy(x => x.Symbol)
                .ToList();
        }

        public ExchangeRateDTO? GetBySymbol(string symbol)
        {
            _rates.TryGetValue(symbol, out var rate);
            return rate;
        }

        public ExchangeRateDTO AddOrUpdate(
            string symbol,
            decimal price,
            DateTimeOffset updatedAt)
        {
            return _rates.AddOrUpdate(
                symbol,
                _ => CreateNewRate(symbol, price, updatedAt),
                (_, existing) => UpdateExistingRate(
                    existing,
                    price,
                    updatedAt));
        }

        private static ExchangeRateDTO CreateNewRate(
            string symbol,
            decimal price,
            DateTimeOffset updatedAt)
        {
            var currencies = ParseCurrencies(symbol);

            return new ExchangeRateDTO
            {
                Symbol = symbol,
                BaseCurrency = currencies.BaseCurrency,
                QuoteCurrency = currencies.QuoteCurrency,
                Price = price,
                PreviousPrice = null,
                Change = 0,
                ChangePercentage = 0,
                IsIncreasing = false,
                IsDecreasing = false,
                UpdatedAt = updatedAt,
                Source = "Finnhub"
            };
        }

        private static ExchangeRateDTO UpdateExistingRate(
            ExchangeRateDTO existing,
            decimal newPrice,
            DateTimeOffset updatedAt)
        {
            var previousPrice = existing.Price;
            var change = newPrice - previousPrice;

            var changePercentage = previousPrice == 0
                ? 0
                : change / previousPrice * 100;

            return new ExchangeRateDTO
            {
                Symbol = existing.Symbol,
                BaseCurrency = existing.BaseCurrency,
                QuoteCurrency = existing.QuoteCurrency,
                Price = newPrice,
                PreviousPrice = previousPrice,
                Change = change,
                ChangePercentage = changePercentage,
                IsIncreasing = change > 0,
                IsDecreasing = change < 0,
                UpdatedAt = updatedAt,
                Source = existing.Source
            };
        }

        private static (string BaseCurrency, string QuoteCurrency)
            ParseCurrencies(string symbol)
        {
            // OANDA:USD_TRY -> USD_TRY
            var cleanSymbol = symbol.Contains(':')
                ? symbol.Split(':', 2)[1]
                : symbol;

            var parts = cleanSymbol.Split(
                '_',
                StringSplitOptions.RemoveEmptyEntries);

            return parts.Length == 2
                ? (parts[0], parts[1])
                : (cleanSymbol, string.Empty);
        }

        public IReadOnlyCollection<ExchangeRateDTO> CalculateMissingRates(DateTimeOffset updatedAt)
        {
            var calculatedRates = new List<ExchangeRateDTO>();

            var eurUsd = GetBySymbol("OANDA:EUR_USD");
            var usdTry = GetBySymbol("OANDA:USD_TRY");
            var eurTry = GetBySymbol("OANDA:EUR_TRY");

            if (eurTry is null &&
                eurUsd is not null &&
                usdTry is not null)
            {
                calculatedRates.Add(
                    AddOrUpdate(
                        "OANDA:EUR_TRY",
                        eurUsd.Price * usdTry.Price,
                        updatedAt));
            }

            if (usdTry is null &&
                eurTry is not null &&
                eurUsd is not null &&
                eurUsd.Price != 0)
            {
                calculatedRates.Add(
                    AddOrUpdate(
                        "OANDA:USD_TRY",
                        eurTry.Price / eurUsd.Price,
                        updatedAt));
            }

            if (eurUsd is null &&
                eurTry is not null &&
                usdTry is not null &&
                usdTry.Price != 0)
            {
                calculatedRates.Add(
                    AddOrUpdate(
                        "OANDA:EUR_USD",
                        eurTry.Price / usdTry.Price,
                        updatedAt));
            }

            return calculatedRates;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Persistence.ExchangeRates
{
    public sealed class FinnhubOptions
    {
        public const string SectionName = "Finnhub";
        public string ApiKey { get; set; } = string.Empty;
        public string RestBaseUrl { get; set; } = string.Empty;
        public string WebSocketUrl { get; set; } = string.Empty;
        public string ForexExchange { get; set; } = "oanda";
        public List<string> Symbols { get; set; } = [];
    }
}

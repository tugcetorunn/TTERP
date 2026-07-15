using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TTERP.Persistence.ExchangeRates.Models
{
    public sealed class FinnhubWebSocketMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public List<FinnhubTradeData>? Data { get; set; }
    }

    public sealed class FinnhubTradeData
    {
        // Fiyat
        [JsonPropertyName("p")]
        public decimal Price { get; set; }

        // Sembol
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;

        // Unix timestamp, milisaniye
        [JsonPropertyName("t")]
        public long Timestamp { get; set; }

        // Hacim
        [JsonPropertyName("v")]
        public decimal Volume { get; set; }
    }
}

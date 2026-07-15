using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.ExchangeRates
{
    public sealed class ExchangeRateDTO
    {
        public string Symbol { get; set; } = string.Empty;
        public string BaseCurrency { get; set; } = string.Empty;
        public string QuoteCurrency { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? PreviousPrice { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercentage { get; set; }
        public bool IsIncreasing { get; set; }
        public bool IsDecreasing { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string Source { get; set; } = "Finnhub";
    }
}

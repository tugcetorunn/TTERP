using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.ServiceInterfaces;
using static System.Net.WebRequestMethods;

namespace TTERP.Persistence.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly IParameterValueRepository _parameterRepository;
        private readonly string todayUrl = "https://www.tcmb.gov.tr/kurlar/today.xml";

        public CurrencyService(HttpClient httpClient, IParameterValueRepository parameterRepository)
        {
            _httpClient = httpClient;
            _parameterRepository = parameterRepository;
        }

        public async Task<decimal> GetExchangeRateAsync(int fromCurrencyId, int toCurrencyId, DateTime date, CancellationToken cancellationToken = default)
        {
            if (fromCurrencyId == toCurrencyId) return 1m;

            string? fromCode = await _parameterRepository.ParamCodeToParamValue("Currency", fromCurrencyId, 1, cancellationToken);
            string? toCode = await _parameterRepository.ParamCodeToParamValue("Currency", toCurrencyId, 1, cancellationToken);

            date = AdjustToWorkingDay(date);

            string url = BuildTcmbUrl(date);

            try
            { 
                var response = await _httpClient.GetAsync(url, cancellationToken); // xml i çekiyoruz.

                // eğer o günkü kurlar bulunamazsa, tatilse...
                if (!response.IsSuccessStatusCode)
                {
                    url = todayUrl;
                    response = await _httpClient.GetAsync(url, cancellationToken);
                    response.EnsureSuccessStatusCode();
                }

                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var xmlDoc = XDocument.Load(stream);

                // kurları tl bazında okuyoruz. (ondalıklar değişiyor)
                var culture = new CultureInfo("en-US");

                decimal fromRateInTry = 1m; // varsayılan try değeri

                if (fromCode != "TRY") // TRY ye dönüştürüyoruz
                {
                    var fromNode = xmlDoc.Descendants("Currency").FirstOrDefault(x => x.Attribute("Kod")?.Value == fromCode);
                    if (fromNode != null)
                    {
                        // forexSelling (döviz satış) genelde kurumsal firmalar arası baz alınır.
                        string rateStr = fromNode.Element("ForexSelling")?.Value;
                        fromRateInTry = decimal.Parse(rateStr, culture);
                    }
                }

                decimal toRateInTry = 1m;

                if(toCode != "TRY")
                {
                    var toNode = xmlDoc.Descendants("Currency").FirstOrDefault(x => x.Attribute("Kod")?.Value == toCode);
                    if (toNode != null)
                    {
                        string rateStr = toNode.Element("ForexSelling")?.Value;
                        toRateInTry = decimal.Parse(rateStr, culture);
                    }
                }

                return fromRateInTry / toRateInTry;

                // Örnek (USD -> EUR) = USD_TRY (Örn: 35.00) / EUR_TRY (Örn: 38.00) = 0.921
                // Örnek (USD -> TRY) = USD_TRY (35.00) / TRY_TRY (1.00) = 35.00
                // Örnek (TRY -> USD) = TRY_TRY (1.00) / USD_TRY (35.00) = 0.0285
            }
            catch (Exception ex)
            {
                throw new Exception($"TCMB kur verisi alınırken bir hata oluştu. URL: {url}", ex);
            }
        }

        private string BuildTcmbUrl(DateTime date)
        {
            if (date.Date == DateTime.Today)
            {
                return todayUrl;
            }

            // format: https://www.tcmb.gov.tr/kurlar/202607/02072026.xml
            string yearMonth = date.ToString("yyyyMM");
            string dayMonthYear = date.ToString("ddMMyyyy");

            return $"https://www.tcmb.gov.tr/kurlar/{yearMonth}/{dayMonthYear}.xml";
        }

        private DateTime AdjustToWorkingDay(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday) return date.AddDays(-1);
            if (date.DayOfWeek == DayOfWeek.Sunday) return date.AddDays(-2);
            return date;

        }
    }
}

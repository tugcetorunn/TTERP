using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Persistence.SeedData;

namespace TTERP.WebApi.Extensions
{
    public static class CurrencySeedExtension
    {
        public static async Task SeedCurrencyValuesAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var seeder = scope.ServiceProvider
                .GetRequiredService<CurrencyExcelSeeder>();

            var env = scope.ServiceProvider
                .GetRequiredService<IWebHostEnvironment>();

            var filePath = Path.Combine(
                env.ContentRootPath,
                "SeedData",
                "DovizCinsiKodlari.xlsx");

            if (!File.Exists(filePath))
                throw new FileNotFoundException(
                    "Döviz Excel dosyası bulunamadı.",
                    filePath);

            await seeder.SeedCurrenciesAsync(
                filePath,
                languageId: 1);
        }
    }
}

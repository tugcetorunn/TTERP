using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Persistence.Contexts;
using Task = System.Threading.Tasks.Task;

namespace TTERP.Persistence.SeedData
{
    public class CurrencyExcelSeeder
    {
        private readonly AppDbContext _context;

        public CurrencyExcelSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedCurrenciesAsync(string excelFilePath, int languageId = 1)
        {
            var currencyDefinition = await _context.ParameterDefinitions.FirstOrDefaultAsync(x => x.ParamType == "Currency");

            if (currencyDefinition is null)
            {
                currencyDefinition = new ParameterDefinition
                {
                    ParamType = "Currency",
                    DataType = "string",
                    Description = "Uluslararası döviz kodları",
                    DefaultValue = 104
                };

                await _context.ParameterDefinitions.AddAsync(currencyDefinition);
                await _context.SaveChangesAsync();// Id burada oluşur
            } 

            using var workbook = new XLWorkbook(excelFilePath);

            var worksheet = workbook.Worksheets.First();

            var headerRow = worksheet.FirstRowUsed();

            if (headerRow == null)
                throw new Exception("Excel dosyasında başlık satırı bulunamadı.");

            var currencyCodeColumn = headerRow.CellsUsed()
                                              .FirstOrDefault(x => NormalizeHeader(x.GetString()) == NormalizeHeader("dovizKodu"))?
                                              .Address.ColumnNumber;

            var currencyDescriptionColumn = headerRow.CellsUsed()
                                                     .FirstOrDefault(x => NormalizeHeader(x.GetString()) == NormalizeHeader("dovizAciklamasi"))?
                                                     .Address.ColumnNumber;

            if (currencyCodeColumn == null)
                throw new Exception("'Döviz Kodu' kolonu bulunamadı.");

            if (currencyDescriptionColumn == null)
                throw new Exception("'Döviz Açıklaması' kolonu bulunamadı.");

            var existingCurrencyCodes = await _context.ParameterValues
                                                      .Where(x => x.ParameterDefinitionId == currencyDefinition.Id && x.LanguageId == languageId)
                                                      .Select(x => x.ParamValue)
                                                      .ToListAsync();

            var existingCodeSet = existingCurrencyCodes.ToHashSet(StringComparer.OrdinalIgnoreCase);

            var parameterValues = new List<ParameterValue>();

            var currentParamCode = await _context.ParameterValues
                                                 .Where(x => x.ParameterDefinitionId == currencyDefinition.Id && x.LanguageId == languageId)
                                                 .Select(x => (int?)x.ParamCode)
                                                 .MaxAsync() ?? 0;

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                var currencyCode = row.Cell(currencyCodeColumn.Value)
                                      .GetString()
                                      .Trim()
                                      .ToUpperInvariant();

                var currencyDescription = row.Cell(currencyDescriptionColumn.Value)
                                      .GetString()
                                      .Trim();

                // boş satırları atla
                if (string.IsNullOrWhiteSpace(currencyCode))
                    continue;

                // aynı döviz kodu daha önce eklenmişse ekleme
                if (existingCodeSet.Contains(currencyCode))
                    continue;

                currentParamCode++;

                parameterValues.Add(new ParameterValue
                {
                    ParamCode = currentParamCode,
                    ParamValue = currencyCode,
                    Description = string.IsNullOrWhiteSpace(currencyDescription) ? null : currencyDescription,
                    LanguageId = languageId,
                    ParameterDefinitionId = currencyDefinition.Id
                });

                existingCodeSet.Add(currencyCode);
            }

            if (parameterValues.Count == 0)
                return;

            await _context.ParameterValues.AddRangeAsync(parameterValues);
            await _context.SaveChangesAsync();
        }

        private static string NormalizeHeader(string header)
        {
            return header.Trim()
                         .Replace("\r", "")
                         .Replace("\n", "")
                         .Replace(" ", "")
                         .ToLowerInvariant();
        }
    }
}

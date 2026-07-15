using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TTERP.Persistence.Contexts;

namespace TTERP.Persistence.SeedData
{
    public static class LocationSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            var hasData =
                await context.Countries.AnyAsync() ||
                await context.Cities.AnyAsync() ||
                await context.Towns.AnyAsync() ||
                await context.Districts.AnyAsync() ||
                await context.PostalCodes.AnyAsync() ||
                await context.Neighborhoods.AnyAsync();

            if (hasData)
                return;

            var filePath = Path.Combine(
                AppContext.BaseDirectory,
                "SeedData",
                "LocationsInsertScript.txt"
            );

            var lines = await File.ReadAllLinesAsync(filePath);

            Console.WriteLine($"Seed dosyası: {filePath}");
            Console.WriteLine($"11287. satır: {lines[11286]}");

            await using var transaction =
                await context.Database.BeginTransactionAsync();

            try
            {
                for (var i = 0; i < lines.Length; i++)
                {
                    var sql = lines[i].Trim();

                    if (string.IsNullOrWhiteSpace(sql))
                        continue;

                    if (sql.Equals("GO", StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (!sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase) &&
                        !sql.StartsWith("SET IDENTITY_INSERT",
                            StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    try
                    {
                        await context.Database.ExecuteSqlRawAsync(sql);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            $"Seed hatası. Dosya satırı: {i + 1}{Environment.NewLine}" +
                            $"SQL: {sql}",
                            ex
                        );
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

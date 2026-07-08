using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TTERP.Application.Interfaces;
using TTERP.Application.Models.DTOs.Employees;
using TTERP.Domain.Entities;
using TTERP.Persistence.Contexts;

namespace TTERP.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Employee> _userManager;
        private readonly IConfiguration _configuration;
        private static readonly CultureInfo Tr = new("tr-TR");

        public AuthService(AppDbContext context, UserManager<Employee> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> GenerateRandomPasswordAsync()
        {
            const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            const string lower = "abcdefghijkmnpqrstuvwxyz";
            const string digit = "0123456789";
            const string special = ".?!-+&/()*";
            const string all = upper + lower + digit + special;

            var rand = new Random();
            var password = new List<char>
        {
            upper[rand.Next(upper.Length)],
            lower[rand.Next(lower.Length)],
            digit[rand.Next(digit.Length)],
            special[rand.Next(special.Length)]
        };

            while (password.Count < 10)
                password.Add(all[rand.Next(all.Length)]);

            password = password.OrderBy(x => rand.Next()).ToList();
            return string.Concat(password);
        }


        public async Task<string> GenerateEmailAsync(string firstName, string lastName)
        {
            firstName = firstName.Replace(" ", "");
            firstName = firstName.Replace("I", "i");
            lastName = lastName.Replace(" ", "");
            lastName = lastName.Replace("I", "i");
            string baseEmail = $"{Normalize(firstName)}.{Normalize(lastName)}".ToLower();
            string domain = $"{_configuration["CompanyInfo:CompanyName"]!.ToLowerInvariant()}.com.tr";

            int count = 0;
            string finalEmail = $"{baseEmail}@{domain}";

            while (await _context.Users.AnyAsync(u => u.Email == finalEmail))
            {
                count++;
                finalEmail = $"{baseEmail}{count}@{domain}";
            }

            return finalEmail;
        }

        private string Normalize(string value)
        {
            return value.Replace("ç", "c").Replace("ğ", "g").Replace("ş", "s").Replace("ü", "u")
                        .Replace("ö", "o").Replace("ı", "i");
        }

        public string NormalizeUpperName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            var normalized = name
                .Replace("Ç", "C")
                .Replace("Ğ", "G")
                .Replace("İ", "I")
                .Replace("I", "I")
                .Replace("Ö", "O")
                .Replace("Ş", "S")
                .Replace("Ü", "U")
                .Replace("ç", "C")
                .Replace("ğ", "G")
                .Replace("ı", "I")
                .Replace("ö", "O")
                .Replace("ş", "S")
                .Replace("ü", "U");

            return normalized.ToUpperInvariant();
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDTO dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı bulunamadı." });

            var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

            if (!result.Succeeded)
                return result;

            user.IsPasswordChanged = true;
            await _userManager.UpdateAsync(user);

            return IdentityResult.Success;
        }

        public string ToTurkishNameFormatter(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Trim + birden fazla boşluğu teke indir
            var s = Regex.Replace(input.Trim(), @"\\s+", " ");

            // Tireli parçalara ayrı ayrı title-case
            var hyphenParts = s.Split('-');

            for (int i = 0; i < hyphenParts.Length; i++)
            {
                // Türkçeye göre önce tamamen lowercase, sonra TitleCase
                var lower = hyphenParts[i].ToLower(Tr);
                var titled = Tr.TextInfo.ToTitleCase(lower);

                // Apostrof içerenleri (O'CONNOR) de düzgün yapar; yine de normalize edelim:
                // Örn: "o'connor" -> "O'Connor"
                titled = Regex.Replace(
                    titled,
                    @"\\b[\\p{L}]+('[\\p{L}]+)?",
                    m =>
                    {
                        var w = m.Value;
                        if (w.Length == 0) return w;
                        // İlk karakteri TR’ye göre büyüt, kalan TR’ye göre küçük
                        var first = w.Substring(0, 1).ToUpper(Tr);
                        var rest = w.Length > 1 ? w.Substring(1).ToLower(Tr) : string.Empty;
                        return first + rest;
                    });

                hyphenParts[i] = titled;
            }

            return string.Join("-", hyphenParts);
        }

        public string GenerateUsername(string firstname, string lastname)
        {
            return $"{Normalize(firstname)}.{Normalize(lastname)}";
        }
    }
}

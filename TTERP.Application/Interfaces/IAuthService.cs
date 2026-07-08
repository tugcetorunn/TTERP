using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Employees;

namespace TTERP.Application.Interfaces
{
    public interface IAuthService // dto kullandığımız için bu service interface ini domain yerine applicationa yazdık.
    {
        Task<string> GenerateRandomPasswordAsync();
        Task<string> GenerateEmailAsync(string firstName, string lastName);
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDTO dto);
        string? NormalizeUpperName(string? name);
        string ToTurkishNameFormatter(string? input);
        string GenerateUsername(string firstname, string lastname);
    }
}

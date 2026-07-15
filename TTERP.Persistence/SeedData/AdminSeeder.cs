using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TTERP.Persistence.SeedData
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(UserManager<Employee> userManager)
        {
            const string email = "ali.ergul@ttcompany.com.tr";

            var admin = await userManager.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (admin is null)
            {
                admin = new Employee
                {
                    FirstName = "Ali",
                    LastName = "Ergül",
                    Email = email,
                    NormalizedEmail = email.ToUpperInvariant(),

                    UserName = "aliergul",
                    NormalizedUserName = "ALIERGUL",

                    NationalId = "12345678912",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    PhoneNumber = "5551112345",
                    RegistrationNumber = 10001,
                    HireDate = new DateTime(2015, 1, 1),

                    CountryId = 212,
                    CityId = 34,
                    TownId = 829,
                    DistrictId = 2231,
                    NeighborhoodId = 17175,

                    EmailConfirmed = true
                };

                admin.SetCreated(0);

                var createResult = await userManager.CreateAsync(admin, "Admin123*");

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        Environment.NewLine,
                        createResult.Errors.Select(x => x.Description));

                    throw new InvalidOperationException(
                        $"Admin oluşturulamadı:{Environment.NewLine}{errors}");
                }

                await userManager.AddToRoleAsync(admin, "Admin");
                return;
            }

            // Admin Mig1 ile önceden oluşturulduysa lokasyonlarını güncelle.
            admin.CountryId = 212;
            admin.CityId = 34;
            admin.TownId = 829;
            admin.DistrictId = 2231;
            admin.NeighborhoodId = 17175;

            var updateResult = await userManager.UpdateAsync(admin);

            if (!updateResult.Succeeded)
            {
                var errors = string.Join(
                    Environment.NewLine,
                    updateResult.Errors.Select(x => x.Description));

                throw new InvalidOperationException(
                    $"Admin güncellenemedi:{Environment.NewLine}{errors}");
            }
        }
    }
}

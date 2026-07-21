using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Employees.Commands;
using TTERP.Application.Interfaces;
using TTERP.Application.Models.ViewModels;
using TTERP.Application.Validators;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Extensions;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Employees.Handlers
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Response<CreateEmployeeResultVM>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAuthService _authService;
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork, IConfiguration configuration, IAuthService authService, UserManager<Employee> userManager, RoleManager<Role> roleManager)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _authService = authService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Response<CreateEmployeeResultVM>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateEmployeeCommandValidator();
                var validResult = validator.Validate(request);

                if (!validResult.IsValid)
                {
                    return Response<CreateEmployeeResultVM>.Fail(
                        400,
                        "Çalışan bilgileri geçersiz.",
                        validResult.Errors.Select(x => x.ErrorMessage).ToArray());
                }

                var validRoles = new[] { 2, 3, 4 }; // "Yönetici", "Kullanıcı", "Denetçi"

                if (!validRoles.Contains(request.RoleId))
                    return new Response<CreateEmployeeResultVM>
                    {
                        StatusCode = 400,
                        IsSuccess = false,
                        Message = "Geçersiz rol seçimi",
                        Data = null
                    };

                request.FirstName = _authService.ToTurkishNameFormatter(request.FirstName);
                request.LastName = _authService.ToTurkishNameFormatter(request.LastName);

                var employee = request.Adapt<Employee>();

                employee.RegistrationNumber = await GenerateRegistrationNumberAsync(cancellationToken);

                employee.InternalPhone = await GenerateInternalPhoneAsync(cancellationToken);

                employee.Email = await _authService.GenerateEmailAsync(request.FirstName, request.LastName);

                employee.RightToAnnualLeave = CalculateAnnualLeave(request.HireDate) - employee.AnnualLeaveUsed;

                employee.UserName = _authService.GenerateUsername(request.FirstName, request.LastName);

                var password = await _authService.GenerateRandomPasswordAsync();

                var result = await _userManager.CreateAsync(employee, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Response<CreateEmployeeResultVM>.Fail(400, "Kullanıcı oluşturma hatası", errors);
                }

                var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
                if (role == null)
                {
                    return Response<CreateEmployeeResultVM>.Fail(404, "Rol bulunamadı.");
                }

                var roleResult = await _userManager.AddToRoleAsync(employee, role.NormalizedName!);
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return Response<CreateEmployeeResultVM>.Fail(400, "Rol atanırken hata oluştu.", errors);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Response<CreateEmployeeResultVM>.Success(new CreateEmployeeResultVM
                {
                    Employee = employee,
                    InitialPassword = password
                }, 201, "Çalışan başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {

                return ex.ToResponse<CreateEmployeeResultVM>();
            }
        }

        private double CalculateAnnualLeave(DateTime hireDate)
        {
            int yearsWorked = DateTime.Now.Year - hireDate.Year;

            // bu yılki işe giriş yıldönümü gelmediyse 1 yıl çıkarıyoruz
            if (hireDate.Date > DateTime.Today.AddYears(-yearsWorked))
            {
                yearsWorked--;
            }

            if (yearsWorked >= 15) return 30;
            if (yearsWorked >= 10) return 20;

            return 14;
        }

        private async Task<int> GenerateRegistrationNumberAsync(CancellationToken cancellationToken)
        {
            int maxRegNum = await _employeeRepository.GetMaxRegistrationNumberAsync(cancellationToken);

            if (maxRegNum == 0)
            {
                string? configValue = _configuration["EmployeeSettings:StartingRegistrationNumber"];
                return string.IsNullOrEmpty(configValue) ? 10001 : int.Parse(configValue);
            }

            return maxRegNum + 1;
        }

        private async Task<string> GenerateInternalPhoneAsync(CancellationToken cancellationToken)
        {
            string? maxPhoneStr = await _employeeRepository.GetMaxInternalPhoneAsync(cancellationToken);

            if (string.IsNullOrEmpty(maxPhoneStr) || !int.TryParse(maxPhoneStr, out int maxPhone))
            {
                return _configuration["EmployeeSettings:StartingInternalPhone"] ?? "1000";
            }

            return (maxPhone + 1).ToString();
        }
    }
}

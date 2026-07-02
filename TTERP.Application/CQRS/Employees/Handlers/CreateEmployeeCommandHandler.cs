using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Employees.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Employees.Handlers
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Response<int>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Response<int>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = request.Adapt<Employee>();

            employee.RegistrationNumber = await GenerateRegistrationNumberAsync(cancellationToken);

            employee.InternalPhone = await GenerateInternalPhoneAsync(cancellationToken);

            employee.RightToAnnualLeave = CalculateAnnualLeave(request.HireDate);

            await _employeeRepository.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(employee.Id, 201, "Çalışan başarıyla oluşturuldu.");
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

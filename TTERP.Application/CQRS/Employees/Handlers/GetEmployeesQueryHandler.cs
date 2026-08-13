using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Employees.Queries;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Models;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Employees.Handlers
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, Response<IReadOnlyList<GetEmployeesDTO>>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public GetEmployeesQueryHandler(IEmployeeRepository employeeRepository, IParameterValueRepository parameterValueRepository, UserManager<Employee> userManager, RoleManager<Role> roleManager)
        {
            _employeeRepository = employeeRepository;
            _parameterValueRepository = parameterValueRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Response<IReadOnlyList<GetEmployeesDTO>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetEmployeesWithRoleAsync(request.IsActive, request.IsDeleted, cancellationToken);

            var genderValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                            "Gender",
                                            1,
                                            cancellationToken);

            var genderDictionary = genderValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);

            var maritalValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                            "MaritalStatus",
                                            1,
                                            cancellationToken);

            var maritalDictionary = maritalValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);

            foreach (var employee in employees)
            {
                employee.GenderName = employee.Gender.HasValue ? genderDictionary.GetValueOrDefault(employee.Gender.Value) : null;
                employee.MaritalStatusName = employee.MaritalStatus.HasValue ? maritalDictionary.GetValueOrDefault(employee.MaritalStatus.Value) : null;

                var employeeEntity = await _userManager.FindByIdAsync(employee.Id.ToString());

                if (employeeEntity == null)
                {
                    continue;
                }

                var roleNames = await _userManager.GetRolesAsync(employeeEntity);

                var roleName = roleNames.FirstOrDefault();

                employee.RoleName = roleName;

                if (!string.IsNullOrWhiteSpace(roleName))
                {
                    var role = await _roleManager.FindByNameAsync(roleName);

                    employee.RoleId = role?.Id;
                }
            }

            return Response<IReadOnlyList<GetEmployeesDTO>>.Success(employees.ToList());
        }
    }
}

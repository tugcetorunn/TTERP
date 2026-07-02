using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Employees.Queries;
using TTERP.Application.Models.DTOs.Employees;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Employees.Handlers
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, Response<IReadOnlyList<GetEmployeesDTO>>>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeesQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Response<IReadOnlyList<GetEmployeesDTO>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetListWithFilterAsync(
                select: e => e.Adapt<GetEmployeesDTO>(),
                where: e => e.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || e.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetEmployeesDTO>>.Success(employees.ToList());
        }
    }
}

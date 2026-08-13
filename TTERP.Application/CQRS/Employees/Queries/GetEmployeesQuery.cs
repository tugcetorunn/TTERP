using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Models;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Employees.Queries
{
    public class GetEmployeesQuery : IRequest<Response<IReadOnlyList<GetEmployeesDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetEmployeesQuery(bool? isActive, bool? isDeleted)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

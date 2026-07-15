using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Customers;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Customers.Queries
{
    public class GetCustomersQuery : IRequest<Response<IReadOnlyList<GetCustomersDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetCustomersQuery(bool? isActive, bool? isDeleted)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

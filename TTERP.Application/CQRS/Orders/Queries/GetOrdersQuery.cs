using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Orders;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Orders.Queries
{
    public class GetOrdersQuery : IRequest<Response<IReadOnlyList<GetOrdersDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetOrdersQuery(bool? isActive, bool? isDeleted)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.OrderItems;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.OrderItems.Queries
{
    public class GetOrderItemsQuery : IRequest<Response<IReadOnlyList<GetOrderItemsDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetOrderItemsQuery(bool? isActive, bool? isDeleted)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

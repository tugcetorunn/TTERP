using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.OrderItemWarehouses;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.OrderItemWarehouses.Queries
{
    public class OrderItemStockLocationQuery : IRequest<Response<IReadOnlyList<OrderItemStockLocationDTO>>>
    {
        public int OrderItemId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public OrderItemStockLocationQuery(int orderItemId, bool? isActive, bool? isDeleted)
        {
            OrderItemId = orderItemId;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

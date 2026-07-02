using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.OrderItemWarehouses;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.OrderItems.Commands
{
    public class CreateOrderItemCommand : IRequest<Response<int>>
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        // Hangi depodan ne kadar çekileceğinin listesi
        public List<OrderItemStockAllocationDTO> StockAllocations { get; set; }
    }
}

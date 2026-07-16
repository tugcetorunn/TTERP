using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.OrderItems.Commands;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Orders.Commands
{
    public class CreateOrderCommand : IRequest<Response<int>>
    {
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public decimal Discount { get; set; }
        public List<CreateOrderItemCommand>? OrderItems { get; set; } = new();
    }
}

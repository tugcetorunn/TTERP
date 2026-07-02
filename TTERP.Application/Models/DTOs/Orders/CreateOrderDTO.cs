using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.OrderItems.Commands;
using TTERP.Application.Models.DTOs.OrderItems;
using TTERP.Application.Models.DTOs.Payments;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Orders
{
    public class CreateOrderDTO
    {
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public List<CreateOrderItemCommand>? OrderItems { get; set; }
    }
}

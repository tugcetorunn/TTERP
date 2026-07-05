using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.SupplyItems.Commands
{
    public class CreateSupplyItemCommand : IRequest<Response<int>>
    {
        public int MaterialId { get; set; }
        public int WarehouseId { get; set; }
        public double Quantity { get; set; }
        public int Currency { get; set; }
        public decimal ListPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TaxRate { get; set; }
    }
}

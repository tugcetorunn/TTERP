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
        public int SupplierMaterialId { get; set; }
        public int MaterialId { get; set; }
        public int WarehouseId { get; set; }
        public double Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal DiscountRate { get; set; } = 0;
    }
}

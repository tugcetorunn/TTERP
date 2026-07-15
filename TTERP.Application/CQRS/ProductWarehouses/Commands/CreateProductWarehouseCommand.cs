using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductWarehouses.Commands
{
    public class CreateProductWarehouseCommand : IRequest<Response<int>>
    {
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public double Quantity { get; set; }
        public int ReasonForEntryOrExit { get; set; }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Materials.Commands
{
    public class CreateMaterialCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string? Description { get; set; }
        public string Unit { get; set; }
        public decimal CostPrice { get; set; }
        public decimal TaxRate { get; set; }
        public double StockQuantity { get; set; }
    }
}

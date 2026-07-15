using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.SupplierMaterials.Commands
{
    public class CreateSupplierMaterialCommand : IRequest<Response<int>>
    {
        public int SupplierId { get; set; }
        public int MaterialId { get; set; }
        public int Currency { get; set; }
        public decimal ListPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public int? LeadTimeDays { get; set; } 
        public double? MOQ { get; set; }
    }
}

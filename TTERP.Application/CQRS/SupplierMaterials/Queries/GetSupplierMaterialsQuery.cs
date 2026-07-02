using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.SupplierMaterials;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.SupplierMaterials.Queries
{
    public class GetSupplierMaterialsQuery : IRequest<Response<IReadOnlyList<GetSupplierMaterialsDTO>>>
    {
        public int SupplierId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

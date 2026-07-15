using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.SupplierMaterials;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.SupplierMaterials.Queries
{
    public class GetSupplierMaterialsQuery : IRequest<Response<IReadOnlyList<GetSupplierMaterialsDTO>>>
    {
        public int? SupplierId { get; set; }
        public int? MaterialId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetSupplierMaterialsQuery(int? supplierId, int? materialId, bool? isActive, bool? isDeleted)
        {
            SupplierId = supplierId;
            MaterialId = materialId;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

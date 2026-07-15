using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.MaterialWarehouses;
using TTERP.Domain.Entities;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.MaterialWarehouses.Queries
{
    public class GetMaterialsStockQuery : IRequest<Response<IReadOnlyList<GetMaterialsStockDTO>>>
    {
        public int? MaterialId { get; set; }
        public int? WarehouseId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetMaterialsStockQuery(int? materialId, int? warehouseId, bool? isActive, bool? isDeleted)
        {
            MaterialId = materialId;
            WarehouseId = warehouseId;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

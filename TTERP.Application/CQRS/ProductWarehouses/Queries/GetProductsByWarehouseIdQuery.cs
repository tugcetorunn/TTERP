using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductWarehouses.Queries
{
    public class GetProductsByWarehouseIdQuery : IRequest<Response<IReadOnlyList<GetWarehouseToProductsDTO>>>
    {
        public int WarehouseId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetProductsByWarehouseIdQuery(int warehouseId, bool? isActive, bool? isDeleted)
        {
            WarehouseId = warehouseId;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

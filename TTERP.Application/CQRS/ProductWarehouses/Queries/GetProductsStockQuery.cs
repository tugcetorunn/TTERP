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
    public class GetProductsStockQuery : IRequest<Response<IReadOnlyList<GetProductsStockDTO>>>
    {
        public int? ProductId { get; set; }
        public int? WarehouseId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetProductsStockQuery(int? productId, int? warehouseId, bool? isActive, bool? isDeleted)
        {
            ProductId = productId;
            WarehouseId = warehouseId;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

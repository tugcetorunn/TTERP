using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.Warehouses;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Warehouses.Queries
{
    public class GetWarehousesQuery : IRequest<Response<IReadOnlyList<GetWarehousesDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetWarehousesQuery(bool? isActive, bool? isDeleted)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

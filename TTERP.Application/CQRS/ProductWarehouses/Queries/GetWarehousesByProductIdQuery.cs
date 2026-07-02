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
    public class GetWarehousesByProductIdQuery : IRequest<Response<IReadOnlyList<GetProductWarehousesDTO>>>
    {
        public int ProductId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

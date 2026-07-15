using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.SupplyItems;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.SupplyItems.Queries
{
    public class GetSupplyItemsQuery : IRequest<Response<IReadOnlyList<GetSupplyItemsDTO>>>
    {
        public int SupplyId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetSupplyItemsQuery(int supplyId, bool? isActive, bool? isDeleted)
        {
            SupplyId = supplyId;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

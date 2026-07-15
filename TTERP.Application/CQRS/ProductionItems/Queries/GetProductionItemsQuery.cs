using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductionItems;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductionItems.Queries
{
    public class GetProductionItemsQuery : IRequest<Response<IReadOnlyList<GetProductionItemsDTO>>>
    {
        public int ProductionId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetProductionItemsQuery(int productionId, bool? isActive, bool? isDeleted)
        {
            ProductionId = productionId;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

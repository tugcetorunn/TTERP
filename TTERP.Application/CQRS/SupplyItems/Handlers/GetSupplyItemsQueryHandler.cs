using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Queries;
using TTERP.Application.CQRS.SupplyItems.Queries;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.SupplyItems;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.SupplyItems.Handlers
{
    public class GetSupplyItemsQueryHandler : IRequestHandler<GetSupplyItemsQuery, Response<IReadOnlyList<GetSupplyItemsDTO>>>
    {
        private readonly ISupplyItemRepository _supplyItemRepository;

        public GetSupplyItemsQueryHandler(ISupplyItemRepository supplyItemRepository)
        {
            _supplyItemRepository = supplyItemRepository;
        }

        public async Task<Response<IReadOnlyList<GetSupplyItemsDTO>>> Handle(GetSupplyItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _supplyItemRepository.GetListWithFilterAsync(
                i => i.Adapt<GetSupplyItemsDTO>(),
                i => i.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || i.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetSupplyItemsDTO>>.Success(items.ToList());
        }
    }
}

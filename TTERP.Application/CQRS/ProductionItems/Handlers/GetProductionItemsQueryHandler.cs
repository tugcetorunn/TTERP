using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductionItems.Queries;
using TTERP.Application.Models.DTOs.ProductionItems;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductionItems.Handlers
{
    public class GetProductionItemsQueryHandler : IRequestHandler<GetProductionItemsQuery, Response<IReadOnlyList<GetProductionItemsDTO>>>
    {
        private readonly IProductionItemRepository _productionItemRepository;

        public GetProductionItemsQueryHandler(IProductionItemRepository productionItemRepository)
        {
            _productionItemRepository = productionItemRepository;
        }

        public async Task<Response<IReadOnlyList<GetProductionItemsDTO>>> Handle(GetProductionItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _productionItemRepository.GetListWithFilterAsync(
                pi => pi.Adapt<GetProductionItemsDTO>(),
                pi => pi.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || pi.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetProductionItemsDTO>>.Success(items.ToList());
        }
    }
}

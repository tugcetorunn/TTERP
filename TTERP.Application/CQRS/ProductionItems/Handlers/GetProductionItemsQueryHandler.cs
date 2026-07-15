using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            select: pi => pi,
            where: pi =>
                pi.ProductionId == request.ProductionId &&
                pi.IsDeleted == (request.IsDeleted ?? false) &&
                (!request.IsActive.HasValue || pi.IsActive == request.IsActive.Value),
            include: query => query
                .Include(pi => pi.Material)
                .Include(pi => pi.SourceWarehouse)!);

            var result = items
                .Select(item => item.Adapt<GetProductionItemsDTO>())
                .ToList();

            return Response<IReadOnlyList<GetProductionItemsDTO>>.Success(result);
        }
    }
}

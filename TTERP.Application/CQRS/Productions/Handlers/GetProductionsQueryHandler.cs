using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Productions.Queries;
using TTERP.Application.Models.DTOs.Productions;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Productions.Handlers
{
    public class GetProductionsQueryHandler : IRequestHandler<GetProductionsQuery, Response<IReadOnlyList<GetProductionsDTO>>>
    {
        private readonly IProductionRepository _productionRepository;

        public GetProductionsQueryHandler(IProductionRepository productionRepository)
        {
            _productionRepository = productionRepository;
        }

        public async Task<Response<IReadOnlyList<GetProductionsDTO>>> Handle(GetProductionsQuery request, CancellationToken cancellationToken)
        {
            var productions = await _productionRepository.GetListWithFilterAsync(
                p => p.Adapt<GetProductionsDTO>(),
                p => p.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || p.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetProductionsDTO>>.Success(productions.ToList());
        }
    }
}

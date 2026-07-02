using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Queries;
using TTERP.Application.CQRS.Supplies.Queries;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.Supplies;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Supplies.Handlers
{
    public class GetSuppliesQueryHandler : IRequestHandler<GetSuppliesQuery, Response<IReadOnlyList<GetSuppliesDTO>>>
    {
        private readonly ISupplyRepository _supplyRepository;

        public GetSuppliesQueryHandler(ISupplyRepository supplyRepository)
        {
            _supplyRepository = supplyRepository;
        }

        public async Task<Response<IReadOnlyList<GetSuppliesDTO>>> Handle(GetSuppliesQuery request, CancellationToken cancellationToken)
        {
            var supplies = await _supplyRepository.GetListWithFilterAsync(
                s => s.Adapt<GetSuppliesDTO>(),
                s => s.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || s.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetSuppliesDTO>>.Success(supplies.ToList());
        }
    }
}

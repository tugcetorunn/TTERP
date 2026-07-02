using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Queries;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductWarehouses.Handlers
{
    public class GetInventoryListQueryHandler : IRequestHandler<GetInventoryListQuery, Response<IReadOnlyList<GetInventoryListDTO>>>
    {
        private readonly IProductWarehouseRepository _productWarehouseRepository;

        public GetInventoryListQueryHandler(IProductWarehouseRepository productWarehouseRepository)
        {
            _productWarehouseRepository = productWarehouseRepository;
        }

        public async Task<Response<IReadOnlyList<GetInventoryListDTO>>> Handle(GetInventoryListQuery request, CancellationToken cancellationToken)
        {
            var list = await _productWarehouseRepository.GetListWithFilterAsync(
                i => i.Adapt<GetInventoryListDTO>(),
                i => i.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || i.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetInventoryListDTO>>.Success(list.ToList());
        }
    }
}

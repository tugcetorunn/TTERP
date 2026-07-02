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
    public class GetWarehousesByProductIdQueryHandler : IRequestHandler<GetWarehousesByProductIdQuery, Response<IReadOnlyList<GetProductWarehousesDTO>>>
    {
        private readonly IProductWarehouseRepository _productWarehouseRepository;

        public GetWarehousesByProductIdQueryHandler(IProductWarehouseRepository productWarehouseRepository)
        {
            _productWarehouseRepository = productWarehouseRepository;
        }

        public async Task<Response<IReadOnlyList<GetProductWarehousesDTO>>> Handle(GetWarehousesByProductIdQuery request, CancellationToken cancellationToken)
        {
            var warehouses = await _productWarehouseRepository.GetListWithFilterAsync(
                    pw => pw.Adapt<GetProductWarehousesDTO>(),
                    pw => pw.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || pw.IsActive == request.IsActive.Value)
                    && pw.ProductId == request.ProductId);

            return Response<IReadOnlyList<GetProductWarehousesDTO>>.Success(warehouses.ToList());
        }
    }
}

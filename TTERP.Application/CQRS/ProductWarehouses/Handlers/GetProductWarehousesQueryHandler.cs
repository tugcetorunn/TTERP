using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class GetProductWarehousesQueryHandler : IRequestHandler<GetProductWarehousesQuery, Response<IReadOnlyList<GetProductWarehousesDTO>>>
    {
        private readonly IProductWarehouseRepository _productWarehouseRepository;

        public GetProductWarehousesQueryHandler(IProductWarehouseRepository productWarehouseRepository)
        {
            _productWarehouseRepository = productWarehouseRepository;
        }

        public async Task<Response<IReadOnlyList<GetProductWarehousesDTO>>> Handle(GetProductWarehousesQuery request, CancellationToken cancellationToken)
        {
            var list = await _productWarehouseRepository.GetListWithFilterAsync(
                    select: movement => movement,
                    where: movement =>
                        movement.IsDeleted == (request.IsDeleted ?? false) &&
                        (!request.IsActive.HasValue ||
                         movement.IsActive == request.IsActive.Value) &&
                        (!request.ProductId.HasValue ||
                         movement.ProductId == request.ProductId.Value) &&
                        (!request.WarehouseId.HasValue ||
                         movement.WarehouseId == request.WarehouseId.Value),
                    include: query => query
                        .Include(movement => movement.Product)
                        .Include(movement => movement.Warehouse)!);

                            var result = list
                                .Select(movement =>
                                    movement.Adapt<GetProductWarehousesDTO>())
                                .ToList();

            return Response<IReadOnlyList<GetProductWarehousesDTO>>.Success(result);
        }
    }
}

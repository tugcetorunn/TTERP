using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Warehouses.Queries;
using TTERP.Application.Models.DTOs.Warehouses;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Warehouses.Handlers
{
    public class GetWarehousesQueryHandler : IRequestHandler<GetWarehousesQuery, Response<IReadOnlyList<GetWarehousesDTO>>>
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public GetWarehousesQueryHandler(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<Response<IReadOnlyList<GetWarehousesDTO>>> Handle(GetWarehousesQuery request, CancellationToken cancellationToken)
        {
            var warehouses = await _warehouseRepository.GetListWithFilterAsync(
                    w => w.Adapt<GetWarehousesDTO>(),
                    w => w.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || w.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetWarehousesDTO>>.Success(warehouses.ToList());
        }
    }
}

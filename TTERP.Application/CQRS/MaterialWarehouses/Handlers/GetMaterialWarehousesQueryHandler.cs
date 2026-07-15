using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.MaterialWarehouses.Queries;
using TTERP.Application.Models.DTOs.MaterialWarehouses;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.MaterialWarehouses.Handlers
{
    public class GetMaterialWarehousesQueryHandler : IRequestHandler<GetMaterialWarehousesQuery, Response<IReadOnlyList<GetMaterialWarehousesDTO>>>
    {
        private readonly IMaterialWarehouseRepository _materialWarehouseRepository;

        public GetMaterialWarehousesQueryHandler(IMaterialWarehouseRepository materialWarehouseRepository)
        {
            _materialWarehouseRepository = materialWarehouseRepository;
        }

        public async Task<Response<IReadOnlyList<GetMaterialWarehousesDTO>>> Handle(GetMaterialWarehousesQuery request, CancellationToken cancellationToken)
        {
            var result = await _materialWarehouseRepository.GetListWithFilterAsync(
                    select: x => x.Adapt<GetMaterialWarehousesDTO>(),
                    where: x => x.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || x.IsActive == request.IsActive.Value) && (!request.MaterialId.HasValue ||
                         x.MaterialId == request.MaterialId.Value) && (!request.WarehouseId.HasValue || x.WarehouseId == request.WarehouseId.Value),
                    include: x => x
                        .Include(x => x.Material)
                        .Include(x => x.Warehouse)!
                );

            return Response<IReadOnlyList<GetMaterialWarehousesDTO>>
                .Success(result.ToList());
        }
    }
}

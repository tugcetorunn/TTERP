using Mapster;
using MediatR;
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
    public class MaterialStockLocationQueryHandler : IRequestHandler<MaterialStockLocationQuery, Response<IReadOnlyList<MaterialStockLocationDTO>>>
    {
        private readonly IMaterialWarehouseRepository _materialWarehouseRepository;

        public MaterialStockLocationQueryHandler(IMaterialWarehouseRepository materialWarehouseRepository)
        {
            _materialWarehouseRepository = materialWarehouseRepository;
        }

        public async Task<Response<IReadOnlyList<MaterialStockLocationDTO>>> Handle(MaterialStockLocationQuery request, CancellationToken cancellationToken)
        {
            var materialStock = await _materialWarehouseRepository.GetListWithFilterAsync(
                s => s.Adapt<MaterialStockLocationDTO>(),
                s => s.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || s.IsActive == request.IsActive.Value)
                && s.MaterialId == request.MaterialId);

            return Response<IReadOnlyList<MaterialStockLocationDTO>>.Success(materialStock.ToList());
        }
    }
}

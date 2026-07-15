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
using TTERP.Domain.Models;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.MaterialWarehouses.Handlers
{
    public class GetMaterialsStockQueryHandler : IRequestHandler<GetMaterialsStockQuery, Response<IReadOnlyList<GetMaterialsStockDTO>>>
    {
        private readonly IMaterialWarehouseRepository _materialWarehouseRepository;

        public GetMaterialsStockQueryHandler(IMaterialWarehouseRepository materialWarehouseRepository)
        {
            _materialWarehouseRepository = materialWarehouseRepository;
        }

        public async Task<Response<IReadOnlyList<GetMaterialsStockDTO>>> Handle(GetMaterialsStockQuery request, CancellationToken cancellationToken)
        {
            var stocks = await _materialWarehouseRepository.GetMaterialsStockAsync(request.MaterialId, request.WarehouseId, cancellationToken);

            var response = stocks.Adapt<List<GetMaterialsStockDTO>>();

            return Response<IReadOnlyList<GetMaterialsStockDTO>>.Success(response, 200);
        }
    }
}

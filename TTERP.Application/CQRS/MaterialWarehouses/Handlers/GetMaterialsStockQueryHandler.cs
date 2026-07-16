using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.MaterialWarehouses.Queries;
using TTERP.Application.Models.DTOs.MaterialWarehouses;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Models;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.MaterialWarehouses.Handlers
{
    public class GetMaterialsStockQueryHandler : IRequestHandler<GetMaterialsStockQuery, Response<IReadOnlyList<GetMaterialsStockDTO>>>
    {
        private readonly IMaterialWarehouseRepository _materialWarehouseRepository;
        private readonly IParameterValueRepository _parameterValueRepository;

        public GetMaterialsStockQueryHandler(IMaterialWarehouseRepository materialWarehouseRepository, IParameterValueRepository parameterValueRepository)
        {
            _materialWarehouseRepository = materialWarehouseRepository;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<IReadOnlyList<GetMaterialsStockDTO>>> Handle(GetMaterialsStockQuery request, CancellationToken cancellationToken)
        {
            var stocks = await _materialWarehouseRepository.GetMaterialsStockAsync(request.MaterialId, request.WarehouseId, cancellationToken);

            var response = stocks.Adapt<List<GetMaterialsStockDTO>>();

            var unitValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                            "MaterialUnit",
                                            1,
                                            cancellationToken);

            var unitDictionary = unitValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);

            foreach (var stock in stocks)
            {
                stock.MaterialUnitName = unitDictionary.GetValueOrDefault(stock.MaterialUnit)!;
            }

            return Response<IReadOnlyList<GetMaterialsStockDTO>>.Success(response, 200);
        }
    }
}

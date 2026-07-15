using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Materials.Queries;
using TTERP.Application.Models.DTOs.Materials;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Materials.Handlers
{
    public class GetMaterialsQueryHandler : IRequestHandler<GetMaterialsQuery, Response<IReadOnlyList<GetMaterialsDTO>>>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IParameterValueRepository _parameterValueRepository;

        public GetMaterialsQueryHandler(IMaterialRepository materialRepository, IParameterValueRepository parameterValueRepository)
        {
            _materialRepository = materialRepository;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<IReadOnlyList<GetMaterialsDTO>>> Handle(GetMaterialsQuery request, CancellationToken cancellationToken)
        {
            var materials = await _materialRepository.GetListWithFilterAsync(
                select: m => m.Adapt<GetMaterialsDTO>(),
                where: m => m.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || m.IsActive == request.IsActive.Value));

            var materialIds = materials.Select(material => material.Id).ToList();

            var supplierCounts = await _materialRepository.GetSupplierCountOfMaterialsAsync(materialIds);

            var warehouseCounts = await _materialRepository.GetWarehouseCountOfMaterialsAsync(materialIds);

            var unitValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                            "MaterialUnit",
                                            1,
                                            cancellationToken);

            var unitDictionary = unitValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);

            foreach (var material in materials)
            {
                material.SupplierCount = supplierCounts.GetValueOrDefault(material.Id, 0);
                material.WarehouseCount = warehouseCounts.GetValueOrDefault(material.Id, 0);
                material.UnitName = unitDictionary.GetValueOrDefault(material.Unit);
            }

            return Response<IReadOnlyList<GetMaterialsDTO>>.Success(materials.ToList());
        }
    }
}

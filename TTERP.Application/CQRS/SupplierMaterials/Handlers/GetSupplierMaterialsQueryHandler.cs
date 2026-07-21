using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.SupplierMaterials.Queries;
using TTERP.Application.Models.DTOs.MaterialWarehouses;
using TTERP.Application.Models.DTOs.SupplierMaterials;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.SupplierMaterials.Handlers
{
    public class GetSupplierMaterialsQueryHandler : IRequestHandler<GetSupplierMaterialsQuery, Response<IReadOnlyList<GetSupplierMaterialsDTO>>>
    {
        private readonly ISupplierMaterialRepository _supplierMaterialRepository;
        private readonly IParameterValueRepository _parameterValueRepository;

        public GetSupplierMaterialsQueryHandler(ISupplierMaterialRepository supplierMaterialRepository, IParameterValueRepository parameterValueRepository)
        {
            _supplierMaterialRepository = supplierMaterialRepository;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<IReadOnlyList<GetSupplierMaterialsDTO>>> Handle(GetSupplierMaterialsQuery request, CancellationToken cancellationToken)
        {
            var results = await _supplierMaterialRepository.GetListWithFilterAsync(
                    select: x => x.Adapt<GetSupplierMaterialsDTO>(),
                    where: x => x.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || x.IsActive == request.IsActive.Value) && (!request.MaterialId.HasValue ||
                         x.MaterialId == request.MaterialId.Value) && (!request.SupplierId.HasValue || x.SupplierId == request.SupplierId.Value),
                    include: x => x
                        .Include(x => x.Material)
                        .Include(x => x.Supplier)!
                );

            var unitValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                            "MaterialUnit",
                                            1,
                                            cancellationToken);

            var unitDictionary = unitValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);

            var currencyValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                            "Currency",
                                            1,
                                            cancellationToken);

            var currencyDictionary = currencyValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);


            foreach (var result in results)
            {
                result.MaterialUnitName = unitDictionary.GetValueOrDefault(result.MaterialUnit)!;
                result.CurrencyName = currencyDictionary.GetValueOrDefault(result.Currency)!;
            }

            return Response<IReadOnlyList<GetSupplierMaterialsDTO>>.Success(results.ToList());
        }
    }
}

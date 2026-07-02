using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Queries;
using TTERP.Application.CQRS.SupplierMaterials.Queries;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.SupplierMaterials;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.SupplierMaterials.Handlers
{
    public class GetSupplierMaterialsQueryHandler : IRequestHandler<GetSupplierMaterialsQuery, Response<IReadOnlyList<GetSupplierMaterialsDTO>>>
    {
        private readonly ISupplierMaterialRepository _supplierMaterialRepository;

        public GetSupplierMaterialsQueryHandler(ISupplierMaterialRepository supplierMaterialRepository)
        {
            _supplierMaterialRepository = supplierMaterialRepository;
        }

        public async Task<Response<IReadOnlyList<GetSupplierMaterialsDTO>>> Handle(GetSupplierMaterialsQuery request, CancellationToken cancellationToken)
        {
            var materials = await _supplierMaterialRepository.GetListWithFilterAsync(
                sm => sm.Adapt<GetSupplierMaterialsDTO>(),
                sm => sm.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || sm.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetSupplierMaterialsDTO>>.Success(materials.ToList());
        }
    }
}

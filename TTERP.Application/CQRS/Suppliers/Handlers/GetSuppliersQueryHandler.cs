using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Queries;
using TTERP.Application.CQRS.Suppliers.Queries;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.Suppliers;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Suppliers.Handlers
{
    public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, Response<IReadOnlyList<GetSuppliersDTO>>>
    {
        private readonly ISupplierRepository _supplierRepository;

        public GetSuppliersQueryHandler(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Response<IReadOnlyList<GetSuppliersDTO>>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await _supplierRepository.GetListWithFilterAsync(
                s => s.Adapt<GetSuppliersDTO>(),
                s => s.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || s.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetSuppliersDTO>>.Success(suppliers.ToList());
        }
    }
}

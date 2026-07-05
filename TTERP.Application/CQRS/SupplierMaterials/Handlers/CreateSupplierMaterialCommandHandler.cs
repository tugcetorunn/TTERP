using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.SupplierMaterials.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.SupplierMaterials.Handlers
{
    public class CreateSupplierMaterialCommandHandler : IRequestHandler<CreateSupplierMaterialCommand, Response<int>>
    {
        private readonly ISupplierMaterialRepository _supplierMaterialRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSupplierMaterialCommandHandler(ISupplierMaterialRepository supplierMaterialRepository, ISupplierRepository supplierRepository, IMaterialRepository materialRepository, IUnitOfWork unitOfWork)
        {
            _supplierMaterialRepository = supplierMaterialRepository;
            _supplierRepository = supplierRepository;
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(CreateSupplierMaterialCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierRepository.FindAsync(request.SupplierId);

            if (supplier == null)
            {
                return Response<int>.Fail(404, $"{request.SupplierId} nolu tedarikçi bulunamadı.");
            }

            var material = await _materialRepository.FindAsync(request.MaterialId);

            if (material == null)
            {
                return Response<int>.Fail(404, $"{request.MaterialId} nolu malzeme bulunamadı.");
            }

            var supplierMaterial = request.Adapt<SupplierMaterial>();

            await _supplierMaterialRepository.AddAsync(supplierMaterial);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(supplierMaterial.Id, 201, "Malzeme tedarikçi durumu başarıyla oluşturuldu.");
        }
    }
}

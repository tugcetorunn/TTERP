using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Suppliers.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Suppliers.Handlers
{
    public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, Response<int>>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSupplierCommandHandler(ISupplierRepository supplierRepository, IUnitOfWork unitOfWork)
        {
            _supplierRepository = supplierRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = request.Adapt<Supplier>();

            await _supplierRepository.AddAsync(supplier);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(supplier.Id, 201, "Tedarikçi başarıyla oluşturuldu.");
        }
    }
}

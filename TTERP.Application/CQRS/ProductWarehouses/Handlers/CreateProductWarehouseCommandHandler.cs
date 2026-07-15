using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductWarehouses.Handlers
{
    public class CreateProductWarehouseCommandHandler : IRequestHandler<CreateProductWarehouseCommand, Response<int>>
    {
        private readonly IProductWarehouseRepository _productWarehouseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductWarehouseCommandHandler(IProductWarehouseRepository productWarehouseRepository, IUnitOfWork unitOfWork)
        {
            _productWarehouseRepository = productWarehouseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(CreateProductWarehouseCommand request, CancellationToken cancellationToken)
        {
            var productWarehouse = request.Adapt<ProductWarehouse>();

            await _productWarehouseRepository.AddAsync(productWarehouse);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(productWarehouse.Id, 201, "Ürün depo giriş/çıkış başarıyla oluşturuldu.");
        }
    }
}

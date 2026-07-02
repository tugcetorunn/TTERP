using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.MaterialWarehouses.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.MaterialWarehouses.Handlers
{
    // supply dan warehouse a malzme girişi de olabilir manuel malzeme girişi de olabilir, burası manuel giriş için olacak
    public class CreateMaterialWarehouseCommandHandler : IRequestHandler<CreateMaterialWarehouseCommand, Response<int>>
    {
        private readonly IMaterialWarehouseRepository _materialWarehouseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMaterialWarehouseCommandHandler(IMaterialWarehouseRepository materialWarehouseRepository, IUnitOfWork unitOfWork)
        {
            _materialWarehouseRepository = materialWarehouseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(CreateMaterialWarehouseCommand request, CancellationToken cancellationToken)
        {
            var materialWarehouse = request.Adapt<MaterialWarehouse>();

            await _materialWarehouseRepository.AddAsync(materialWarehouse);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(materialWarehouse.Id, 201, "Malzeme depo giriş/çıkış başarıyla oluşturuldu.");
        }
    }
}

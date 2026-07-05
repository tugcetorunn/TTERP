using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Warehouses.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Warehouses.Handlers
{
    public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, Response<int>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWarehouseCommandHandler(IWarehouseRepository warehouseRepository, IUnitOfWork unitOfWork)
        {
            _warehouseRepository = warehouseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var warehouse = request.Adapt<Warehouse>();

            await _warehouseRepository.AddAsync(warehouse);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(warehouse.Id, 201, "Depo başarıyla oluşturuldu.");
        }
    }
}

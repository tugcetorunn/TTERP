using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Supplies.Commands;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Supplies.Handlers
{
    public class CompleteSupplyCommandHandler : IRequestHandler<CompleteSupplyCommand, Response<int>>
    {
        private readonly ISupplyRepository _supplyRepository;
        private readonly IMaterialWarehouseRepository _materialWarehouseRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteSupplyCommandHandler(ISupplyRepository supplyRepository, IMaterialWarehouseRepository materialWarehouseRepository, IUnitOfWork unitOfWork, IParameterValueRepository parameterValueRepository)
        {
            _supplyRepository = supplyRepository;
            _materialWarehouseRepository = materialWarehouseRepository;
            _unitOfWork = unitOfWork;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<int>> Handle(CompleteSupplyCommand request, CancellationToken cancellationToken)
        {
            var supply = await _supplyRepository.GetSupplyWithItemsAsync(request.SupplyId, cancellationToken);

            if (supply == null)
            {
                return Response<int>.Fail(404, "Tedarik kaydı bulunamadı.");
            }

            var deliveredParamCode = await _parameterValueRepository.ParamValueToParamCode("SupplyStatus", "Delivered", cancellationToken);
            var entryParamCode = await _parameterValueRepository.ParamValueToParamCode("ReasonForEntryOrExit", "Material Input", cancellationToken);

            if (supply.SupplyStatus == deliveredParamCode)
            {
                return Response<int>.Fail(400, "Bu tedarik işlemi zaten tamamlanmış ve mallar depoya girmiş.");
            }

            supply.SupplyStatus = deliveredParamCode;
            supply.DeliveryDate = DateTime.UtcNow;
            supply.DocumentNumber = request.DocumentNumber;

            if (supply.SupplyItems != null && supply.SupplyItems.Any())
            {
                foreach (var item in supply.SupplyItems)
                {
                    await _materialWarehouseRepository.IncreaseStockAsync(
                        warehouseId: item.WarehouseId,
                        materialId: item.MaterialId,
                        quantity: item.Quantity,
                        reason: entryParamCode,
                        cancellationToken);
                }
            }

            _supplyRepository.Update(supply);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(supply.Id, 200, "Tedarik işlemi başarıyla tamamlandı ve malzemeler stoklara eklendi.");
        }
    }
}

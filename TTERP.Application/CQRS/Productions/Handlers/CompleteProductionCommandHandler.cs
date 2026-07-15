using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Productions.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Productions.Handlers
{
    public class CompleteProductionCommandHandler : IRequestHandler<CompleteProductionCommand, Response<int>>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IProductWarehouseRepository _productWarehouseRepository;
        private readonly IMaterialWarehouseRepository _materialWarehouseRepository;
        private readonly IMaterialStockReservationRepository _materialStockReservationRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteProductionCommandHandler(IProductionRepository productionRepository, IProductWarehouseRepository productWarehouseRepository, IUnitOfWork unitOfWork, IParameterValueRepository parameterValueRepository, IMaterialWarehouseRepository materialWarehouseRepository, IMaterialStockReservationRepository materialStockReservationRepository)
        {
            _productionRepository = productionRepository;
            _productWarehouseRepository = productWarehouseRepository;
            _unitOfWork = unitOfWork;
            _parameterValueRepository = parameterValueRepository;
            _materialWarehouseRepository = materialWarehouseRepository;
            _materialStockReservationRepository = materialStockReservationRepository;
        }

        public async Task<Response<int>> Handle(CompleteProductionCommand request, CancellationToken cancellationToken)
        {
            var production = await _productionRepository.GetProductionWithItemsAsync(request.ProductionId, cancellationToken);

            if (production == null)
            {
                return Response<int>.Fail(404, "Üretim emri bulunamadı.");
            }

            if (production.ProductionStatus == await _parameterValueRepository.ParamValueToParamCode("ProductionStatus", "Completed", cancellationToken))
            {
                return Response<int>.Fail(400, "Üretim emri zaten tamamlanmış.");
            }

            if (production.ProductionStatus != await _parameterValueRepository.ParamValueToParamCode("ProductionStatus", "Started", cancellationToken))
            {
                return Response<int>.Fail(400, "Sadece 'Üretimde' durumundaki işler tamamlanabilir.");
            }

            production.ActualQuantity = request.ActualQuantity;
            production.ProductionStatus = await _parameterValueRepository.ParamValueToParamCode("ProductionStatus", "Completed", cancellationToken);

            var reservations = await _materialStockReservationRepository.GetByProductionIdAsync(
                                                                        production.Id,
                                                                        cancellationToken);

            var materialExitReason = await _parameterValueRepository
                .GetByShortCodeAsync(
                    "ReasonForEntryOrExit",
                    "HMDCKS",
                    1,
                    cancellationToken);

            var materialReturnReason = await _parameterValueRepository
                .GetByShortCodeAsync(
                    "ReasonForEntryOrExit",
                    "HMDGRS",
                    1,
                    cancellationToken);

            if (request.CompleteProductionItems != null && request.CompleteProductionItems.Any())
            {
                foreach (var itemRequest in request.CompleteProductionItems)
                {
                    var productionItem = production.ProductionItems!
                        .FirstOrDefault(x => x.Id == itemRequest.ProductionItemId);

                    if (productionItem == null)
                    {
                        return Response<int>.Fail(
                            404,
                            "Üretim malzeme kalemi bulunamadı.");
                    }

                    var reservation = reservations.FirstOrDefault(x =>
                        x.ProductionItemId == productionItem.Id);

                    if (reservation == null)
                    {
                        return Response<int>.Fail(
                            400,
                            "Üretim kalemine ait stok rezervasyonu bulunamadı.");
                    }

                    var actualConsumption =
                        itemRequest.ActualQuantity +
                        itemRequest.ScrapQuantity;

                    var alreadyConsumed = reservation.ConsumedQuantity;
                    var difference = actualConsumption - alreadyConsumed;

                    if (difference > 0)
                    {
                        await _materialWarehouseRepository.DecreaseStockAsync(
                            warehouseId: productionItem.SourceWarehouseId,
                            materialId: productionItem.MaterialId,
                            quantity: difference,
                            reason: materialExitReason!.ParamCode,
                            cancellationToken: cancellationToken);
                    }
                    else if (difference < 0)
                    {
                        await _materialWarehouseRepository.IncreaseStockAsync(
                            warehouseId: productionItem.SourceWarehouseId,
                            materialId: productionItem.MaterialId,
                            quantity: Math.Abs(difference),
                            reason: materialReturnReason!.ParamCode,
                            cancellationToken: cancellationToken);
                    }

                    reservation.ConsumedQuantity = actualConsumption;
                    reservation.IsReleased = true;
                    reservation.ReleasedDate = DateTime.UtcNow;

                    productionItem.ActualQuantity = itemRequest.ActualQuantity;
                    productionItem.ScrapQuantity = itemRequest.ScrapQuantity;
                }
            }

            await _productWarehouseRepository.IncreaseStockAsync(
                production.TargetWarehouseId, 
                production.ProductId, 
                request.ActualQuantity, 
                await _parameterValueRepository.ParamValueToParamCode("ReasonForEntryOrExit", "Production Input"), // üretim çıktısı için stok hareketi türü
                cancellationToken);

            _productionRepository.Update(production);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(production.Id, 200, "Üretim emri başarıyla tamamlandı.");
        }
    }
}
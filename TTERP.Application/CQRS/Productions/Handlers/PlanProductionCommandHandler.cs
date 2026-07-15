using Mapster;
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
    public class PlanProductionCommandHandler : IRequestHandler<PlanProductionCommand, Response<int>>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IMaterialWarehouseRepository _materialWarehouseRepository;
        private readonly IMaterialStockReservationRepository _materialStockReservationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlanProductionCommandHandler(IProductionRepository productionRepository, IUnitOfWork unitOfWork, IParameterValueRepository parameterValueRepository, IMaterialWarehouseRepository materialWarehouseRepository, IMaterialStockReservationRepository materialStockReservationRepository)
        {
            _productionRepository = productionRepository;
            _unitOfWork = unitOfWork;
            _parameterValueRepository = parameterValueRepository;
            _materialWarehouseRepository = materialWarehouseRepository;
            _materialStockReservationRepository = materialStockReservationRepository;
        }

        public async Task<Response<int>> Handle(PlanProductionCommand request, CancellationToken cancellationToken)
        {
            if (request.PlannedQuantity <= 0)
            {
                return Response<int>.Fail(
                    400,
                    "Planlanan üretim miktarı sıfırdan büyük olmalıdır.");
            }

            if (request.ProductionItems == null ||
                !request.ProductionItems.Any())
            {
                return Response<int>.Fail(
                    400,
                    "Üretim için en az bir malzeme kalemi eklenmelidir.");
            }

            if (request.ProductionItems.Any(item => item.PlannedQuantity <= 0))
            {
                return Response<int>.Fail(
                    400,
                    "Malzeme miktarları sıfırdan büyük olmalıdır.");
            }

            var plannedStatus = await _parameterValueRepository
                .GetByParamTypeAndCodeAsync(
                    "ProductionStatus",
                    1,
                    1,
                    cancellationToken);

            if (plannedStatus == null)
            {
                return Response<int>.Fail(
                    500,
                    "Üretim başlangıç durumu bulunamadı.");
            }

            var production = new Production
            {
                ProductId = request.ProductId,
                PlannedQuantity = request.PlannedQuantity,
                TargetWarehouseId = request.TargetWarehouseId,
                ProductionDate = request.ProductionDate,
                ProductionStatus = plannedStatus.ParamCode
            };

            foreach (var itemRequest in request.ProductionItems)
            {
                var stockSummary = await _materialWarehouseRepository.GetStockSummaryAsync(
                                    itemRequest.SourceWarehouseId,
                                    itemRequest.MaterialId,
                                    cancellationToken);

                if (stockSummary.AvailableStock < itemRequest.PlannedQuantity)
                {
                    return Response<int>.Fail(
                        400,
                        $"Malzeme stoğu yetersiz. " +
                        $"Fiziksel Stok: {stockSummary.PhysicalStock}, " +
                        $"Rezerve: {stockSummary.ReservedStock}, " +
                        $"Kullanılabilir: {stockSummary.AvailableStock}, " +
                        $"Talep: {itemRequest.PlannedQuantity}");
                }

                if (stockSummary.AvailableStock < itemRequest.PlannedQuantity)
                {
                    return Response<int>.Fail(
                        400,
                        $"Malzeme için yeterli kullanılabilir stok yok. " +
                        $"Fiziksel stok: {stockSummary.PhysicalStock}, " +
                        $"Rezerve stok: {stockSummary.ReservedStock}, " +
                        $"Kullanılabilir stok: {stockSummary.AvailableStock}, " +
                        $"Talep edilen: {itemRequest.PlannedQuantity}.");
                }

                var productionItem = new ProductionItem
                {
                    MaterialId = itemRequest.MaterialId,
                    SourceWarehouseId = itemRequest.SourceWarehouseId,
                    Quantity = itemRequest.PlannedQuantity,
                    ScrapQuantity = 0
                };

                production.ProductionItems!.Add(productionItem);
            }

            await _productionRepository.AddAsync(production);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            foreach (var item in production.ProductionItems!)
            {
                var reservation = new MaterialStockReservation
                {
                    ProductionId = production.Id,
                    ProductionItemId = item.Id,
                    MaterialId = item.MaterialId,
                    WarehouseId = item.SourceWarehouseId,
                    ReservedQuantity = item.Quantity,
                    ConsumedQuantity = 0,
                    IsReleased = false
                };

                await _materialStockReservationRepository.AddAsync(reservation);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(
                production.Id,
                201,
                "Üretim planı başarıyla oluşturuldu.");
        }
    }
}

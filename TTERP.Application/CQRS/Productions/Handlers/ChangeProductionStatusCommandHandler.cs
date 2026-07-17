using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Productions.Commands;
using TTERP.Application.Interfaces;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Productions.Handlers
{
    public class ChangeProductionStatusCommandHandler
    : IRequestHandler<ChangeProductionStatusCommand, Response<int>>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IProductionItemRepository _productionItemRepository;
        private readonly IWorkflowService _workflowService;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IMaterialWarehouseRepository _materialWarehouseRepository;
        private readonly IProductWarehouseRepository _productWarehouseRepository;
        private readonly IWorkflowHistoryRepository _workflowHistoryRepository;
        private readonly IMaterialStockReservationRepository _materialStockReservationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeProductionStatusCommandHandler(
            IProductionRepository productionRepository,
            IWorkflowService workflowService,
            IParameterValueRepository parameterValueRepository,
            IMaterialWarehouseRepository materialWarehouseRepository,
            IProductWarehouseRepository productWarehouseRepository,
            IUnitOfWork unitOfWork,
            IWorkflowHistoryRepository workflowHistoryRepository,
            IHttpContextAccessor httpContextAccessor,
            IMaterialStockReservationRepository materialStockReservationRepository,
            IProductionItemRepository productionItemRepository)
        {
            _productionRepository = productionRepository;
            _workflowService = workflowService;
            _parameterValueRepository = parameterValueRepository;
            _materialWarehouseRepository = materialWarehouseRepository;
            _productWarehouseRepository = productWarehouseRepository;
            _unitOfWork = unitOfWork;
            _workflowHistoryRepository = workflowHistoryRepository;
            _httpContextAccessor = httpContextAccessor;
            _materialStockReservationRepository = materialStockReservationRepository;
            _productionItemRepository = productionItemRepository;
        }

        public async Task<Response<int>> Handle(
            ChangeProductionStatusCommand request,
            CancellationToken cancellationToken)
        {
            var production = await _productionRepository.GetProductionWithItemsAsync(
                request.ProductionId,
                cancellationToken);

            if (production == null)
            {
                return Response<int>.Fail(404, "Üretim emri bulunamadı.");
            }

            if (!production.ProductionStatus.HasValue)
            {
                return Response<int>.Fail(
                    400,
                    "Üretim durum bilgisi bulunamadı.");
            }

            var employeeId = 0;

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out employeeId))
            {
                return Response<int>.Fail(401, "Kullanıcı bilgisi alınamadı.");
            }

            var transition = await _workflowService.ValidateTransitionAsync(
                workflowType: 2,
                fromStatusCode: production.ProductionStatus.Value,
                toStatusCode: request.TargetStatusCode,
                cancellationToken: cancellationToken);

            if (transition == null)
            {
                return Response<int>.Fail(
                    400,
                    "Bu üretim durum geçişine izin verilmiyor.");
            }

            var targetStatus = await _parameterValueRepository.GetByParamTypeAndCodeAsync(
                    "ProductionStatus",
                    request.TargetStatusCode,
                    1,
                    cancellationToken);

            var materialReturnReason = await _parameterValueRepository.GetByShortCodeAsync(
                    "ReasonForEntryOrExit",
                    "HMDGRS",
                    1,
                    cancellationToken
                );

            if (targetStatus == null)
            {
                return Response<int>.Fail(
                    400,
                    "Hedef üretim durumu bulunamadı.");
            }

            var previousStatusCode = production.ProductionStatus.Value;

            var shortCode = targetStatus.ShortCode?
                .Trim()
                .ToLowerInvariant();

            if (shortCode == "cancelled")
            {
                var reservations = await _materialStockReservationRepository
                    .GetByProductionIdAsync(
                        production.Id,
                        cancellationToken);

                foreach (var reservation in reservations.Where(x => !x.IsReleased))
                {
                    if (reservation.ConsumedQuantity > 0)
                    {
                        await _materialWarehouseRepository.IncreaseStockAsync(
                            warehouseId: reservation.WarehouseId,
                            materialId: reservation.MaterialId,
                            quantity: reservation.ConsumedQuantity,
                            reason: materialReturnReason!.ParamCode,
                            cancellationToken: cancellationToken);
                    }

                    reservation.IsReleased = true;
                    reservation.ReleasedDate = DateTime.UtcNow;
                }
            }

            if (shortCode == "started" && !production.StartedDate.HasValue)
            {
                var reservations = await _materialStockReservationRepository
                    .GetByProductionIdAsync(
                        production.Id,
                        cancellationToken);

                var consumptionReason = await _parameterValueRepository
                    .GetByShortCodeAsync(
                        "ReasonForEntryOrExit",
                        "HMDCKS",
                        1,
                        cancellationToken);

                if (consumptionReason == null)
                {
                    return Response<int>.Fail(
                        500,
                        "Hammadde çıkış nedeni bulunamadı.");
                }

                foreach (var reservation in reservations.Where(x => !x.IsReleased))
                {
                    var remainingReservation =
                        reservation.ReservedQuantity -
                        reservation.ConsumedQuantity;

                    if (remainingReservation <= 0)
                    {
                        continue;
                    }

                    await _materialWarehouseRepository.DecreaseStockAsync(
                        warehouseId: reservation.WarehouseId,
                        materialId: reservation.MaterialId,
                        quantity: remainingReservation,
                        reason: consumptionReason.ParamCode,
                        cancellationToken: cancellationToken);

                    reservation.ConsumedQuantity += remainingReservation;
                }

                production.StartedDate = DateTime.UtcNow;
            }

            if (shortCode == "completed")
            {
                var completionResult = await CompleteProductionAsync(
                    production,
                    request,
                    cancellationToken);

                if (!completionResult.IsSuccess)
                {
                    return completionResult;
                }

                production.CompletedDate = DateTime.UtcNow;
            }

            production.ProductionStatus = request.TargetStatusCode;

            var history = new WorkflowHistory
            {
                WorkflowType = 2,
                RecordId = production.Id,
                FromStatusCode = previousStatusCode,
                ToStatusCode = request.TargetStatusCode,
                EmployeeId = employeeId,
                Note = request.Note,
                ChangeDate = DateTime.UtcNow
            };

            await _workflowHistoryRepository.AddAsync(history);

            _productionRepository.Update(production);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(
                production.Id,
                200,
                "Üretim durumu başarıyla güncellendi.");
        }

        private async Task<Response<int>> CompleteProductionAsync(Production production, ChangeProductionStatusCommand request, CancellationToken cancellationToken)
        {
            var actualQuantity = production.ActualQuantity ?? 0;

            if (actualQuantity <= 0)
            {
                return Response<int>.Fail(
                    400,
                    "Üretim tamamlanmadan önce en az bir ilerleme kaydı girilmelidir.");
            }

            if (request.ProductionItems == null ||
                !request.ProductionItems.Any())
            {
                return Response<int>.Fail(
                    400,
                    "Gerçekleşen malzeme tüketimleri girilmelidir.");
            }

            var reservations = await _materialStockReservationRepository.GetByProductionIdAsync(production.Id, cancellationToken);

            // hammadde iade
            var materialReturnReason =
                await _parameterValueRepository.GetByShortCodeAsync(
                    "ReasonForEntryOrExit",
                    "HMDGRS",
                    1,
                    cancellationToken);

            var materialConsumptionReason =
                await _parameterValueRepository.GetByShortCodeAsync(
                    "ReasonForEntryOrExit",
                    "HMDCKS",
                    1,
                    cancellationToken);

            var productionOutputReason =
                await _parameterValueRepository.GetByShortCodeAsync(
                    "ReasonForEntryOrExit",
                    "URTGRS",
                    1,
                    cancellationToken);

            if (materialConsumptionReason == null ||
                productionOutputReason == null ||
                materialReturnReason == null)
            {
                return Response<int>.Fail(
                    500,
                    "Üretim stok hareket nedenleri tanımlanmamış.");
            }

            decimal totalMaterialCost = 0m;

            foreach (var itemRequest in request.ProductionItems)
            {
                var reservation = reservations.FirstOrDefault(reservation => reservation.ProductionItemId == itemRequest.ProductionItemId);

                if (reservation == null)
                {
                    return Response<int>.Fail(
                        400,
                        "Üretim kalemine ait stok rezervasyonu bulunamadı.");
                }

                var actualConsumption =
                    itemRequest.ActualQuantity +
                    itemRequest.ScrapQuantity;

                var alreadyConsumed =
                    reservation.ConsumedQuantity;

                var difference =
                    actualConsumption - alreadyConsumed;

                var productionItem = await _productionItemRepository.FindAsync(itemRequest.ProductionItemId);

                if (difference > 0)
                {
                    await _materialWarehouseRepository.DecreaseStockAsync(
                        warehouseId: productionItem.SourceWarehouseId,
                        materialId: productionItem.MaterialId,
                        quantity: difference,
                        reason: materialConsumptionReason.ParamCode,
                        cancellationToken: cancellationToken);
                }
                else if (difference < 0)
                {
                    await _materialWarehouseRepository.IncreaseStockAsync(
                        warehouseId: productionItem.SourceWarehouseId,
                        materialId: productionItem.MaterialId,
                        quantity: Math.Abs(difference),
                        reason: materialReturnReason.ParamCode,
                        cancellationToken: cancellationToken);
                }

                reservation.ConsumedQuantity =
                    actualConsumption;

                reservation.IsReleased = true;
                reservation.ReleasedDate = DateTime.UtcNow;

                // maliyet hesabı
                var material = productionItem.Material;

                if (material?.AverageCost == null)
                {
                    return Response<int>.Fail(
                        400,
                        $"{material?.Name ?? "Malzeme"} için maliyet bilgisi bulunamadı.");
                }

                totalMaterialCost += (decimal)actualConsumption * material.AverageCost.Value;
            }

            var producedQuantity = request.ActualQuantity!.Value;

            var unitProductionCost = totalMaterialCost / (decimal)producedQuantity;

            await _productWarehouseRepository.IncreaseStockAsync(
                warehouseId: production.TargetWarehouseId,
                productId: production.ProductId,
                quantity: actualQuantity,
                reason: productionOutputReason.ParamCode,
                cancellationToken: cancellationToken);

            // maliyeti bulurken ağırlıklı ortalama doğru hesaplanabilmesi için bu üretimdeki miktarı da hesaba katmak için increase stock metodundan sonra bu hesabı yapmak gerek;
            var product = production.Product!;

            var currentStock = (decimal)product.StockQuantity;

            var currentAverageCost = product.CostPrice ?? 0m;

            var incomingQuantity = (decimal)producedQuantity;

            var newAverageCost = ( currentStock * currentAverageCost + incomingQuantity * unitProductionCost ) / (currentStock + incomingQuantity);

            product.CostPrice = newAverageCost;

            return Response<int>.Success(production.Id, 200);
        }
    }
}

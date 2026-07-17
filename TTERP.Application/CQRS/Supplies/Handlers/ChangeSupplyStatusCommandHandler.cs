using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Supplies.Commands;
using TTERP.Application.Interfaces;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Supplies.Handlers
{
    public class ChangeSupplyStatusCommandHandler : IRequestHandler<ChangeSupplyStatusCommand, Response<int>>
    {
        private readonly ISupplyRepository _supplyRepository;
        private readonly IWorkflowService _workflowService;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IMaterialWarehouseRepository _materialWarehouseRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeSupplyStatusCommandHandler(ISupplyRepository supplyRepository, IParameterValueRepository parameterValueRepository, IMaterialWarehouseRepository materialWarehouseRepository, IUnitOfWork unitOfWork, IWorkflowService workflowService, IMaterialRepository materialRepository)
        {
            _supplyRepository = supplyRepository;
            _parameterValueRepository = parameterValueRepository;
            _materialWarehouseRepository = materialWarehouseRepository;
            _unitOfWork = unitOfWork;
            _workflowService = workflowService;
            _materialRepository = materialRepository;
        }

        public async Task<Response<int>> Handle(ChangeSupplyStatusCommand request, CancellationToken cancellationToken)
        {
            var supply = await _supplyRepository.GetSupplyWithItemsAsync(
            request.SupplyId,
            cancellationToken);

            if (supply == null)
            {
                return Response<int>.Fail(404, "Tedarik kaydı bulunamadı.");
            }

            if (!supply.SupplyStatus.HasValue)
            {
                return Response<int>.Fail(400, "Tedarik durum bilgisi bulunamadı.");
            }

            var targetStatus = await _parameterValueRepository.GetByParamTypeAndCodeAsync("SupplyStatus", request.TargetStatusCode, 1, cancellationToken);

            if (targetStatus == null)
            {
                return Response<int>.Fail(400, "Hedef tedarik durumu bulunamadı.");
            }

            var isDelivered = string.Equals(
                targetStatus.ShortCode,
                "delivered",
                StringComparison.OrdinalIgnoreCase);

            if (isDelivered && string.IsNullOrWhiteSpace(request.DocumentNumber))
            {
                return Response<int>.Fail(
                    400,
                    "Tedarik teslim alınırken irsaliye numarası girilmelidir.");
            }

            if (isDelivered)
            {
                supply.DocumentNumber = request.DocumentNumber?.Trim();
                supply.DeliveryDate = DateTime.Now;
            }

            var transition = await _workflowService.ValidateTransitionAsync(
                workflowType: 1,
                fromStatusCode: supply.SupplyStatus.Value,
                toStatusCode: request.TargetStatusCode,
                cancellationToken: cancellationToken);

            if (transition == null)
            {
                return Response<int>.Fail(400, "Bu durum geçişine izin verilmiyor.");
            }

            supply.SupplyStatus = request.TargetStatusCode;

            if (transition.CreatesStockMovement)
            {
                var entryReasonCode = await _parameterValueRepository
                    .ParamValueToParamCode(
                        "ReasonForEntryOrExit",
                        "SupplyEntry",
                        cancellationToken);

                foreach (var item in supply.SupplyItems ?? [])
                {
                    await _materialWarehouseRepository.IncreaseStockAsync(
                        warehouseId: item.WarehouseId,
                        materialId: item.MaterialId,
                        quantity: item.Quantity,
                        reason: entryReasonCode,
                        cancellationToken: cancellationToken);

                    var material = await _materialRepository.FindAsync(item.MaterialId);

                    if (material != null)
                    {
                        var currentStock = material.StockQuantity;
                        var currentCost = material.AverageCost ?? 0m;

                        var incomingQuantity = item.Quantity;
                        var incomingUnitPrice = item.UnitPrice - (item.UnitPrice * item.DiscountRate / 100);

                        var newTotalQuantity =
                            currentStock + incomingQuantity;

                        if (newTotalQuantity > 0)
                        {
                            material.AverageCost =
                                (
                                    (decimal)currentStock * currentCost +
                                    (decimal)incomingQuantity *
                                    incomingUnitPrice
                                )
                                /
                                (decimal)newTotalQuantity;
                        }

                        material.LastPurchasePrice = item.UnitPrice - (item.UnitPrice * item.DiscountRate / 100);
                        _materialRepository.Update(material);
                    }
                }
               
                supply.DeliveryDate = DateTime.UtcNow;
            }

            _supplyRepository.Update(supply);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(
                supply.Id,
                200,
                "Tedarik durumu başarıyla güncellendi.");
        }
    }
}

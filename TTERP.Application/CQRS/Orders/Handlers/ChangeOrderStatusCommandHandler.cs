using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Orders.Commands;
using TTERP.Application.Interfaces;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Orders.Handlers
{
    public class ChangeOrderStatusCommandHandler : IRequestHandler<ChangeOrderStatusCommand, Response<int>>
    {
        private const int SalesWorkflowType = 3;

        private readonly IOrderRepository _orderRepository;
        private readonly IWorkflowService _workflowService;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IProductWarehouseRepository _productWarehouseRepository;
        private readonly IWorkflowHistoryRepository _workflowHistoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeOrderStatusCommandHandler(IOrderRepository orderRepository, IWorkflowService workflowService, IParameterValueRepository parameterValueRepository, IProductWarehouseRepository productWarehouseRepository, IWorkflowHistoryRepository workflowHistoryRepository, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _workflowService = workflowService;
            _parameterValueRepository = parameterValueRepository;
            _productWarehouseRepository = productWarehouseRepository;
            _workflowHistoryRepository = workflowHistoryRepository;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderWithOrderItemsAsync(
                request.OrderId,
                cancellationToken);

            if (order == null)
            {
                return Response<int>.Fail(
                    404,
                    "Sipariş bulunamadı.");
            }

            if (!order.OrderStatus.HasValue)
            {
                return Response<int>.Fail(
                    400,
                    "Sipariş durum bilgisi bulunamadı.");
            }

            var employeeIdClaim = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            if (!int.TryParse(employeeIdClaim, out var employeeId))
            {
                return Response<int>.Fail(
                    401,
                    "Kullanıcı bilgisi alınamadı.");
            }

            var previousStatusCode = order.OrderStatus.Value;

            if (previousStatusCode == request.TargetStatusCode)
            {
                return Response<int>.Fail(
                    400,
                    "Sipariş zaten seçilen durumda.");
            }

            var transition = await _workflowService.ValidateTransitionAsync(
                workflowType: SalesWorkflowType,
                fromStatusCode: previousStatusCode,
                toStatusCode: request.TargetStatusCode,
                cancellationToken: cancellationToken);

            if (transition == null)
            {
                return Response<int>.Fail(
                    400,
                    "Bu sipariş durum geçişine izin verilmiyor.");
            }

            var targetStatus = await _parameterValueRepository
                .GetByParamTypeAndCodeAsync(
                    "OrderStatus",
                    request.TargetStatusCode,
                    1,
                    cancellationToken);

            if (targetStatus == null)
            {
                return Response<int>.Fail(
                    400,
                    "Hedef sipariş durumu bulunamadı.");
            }

            var targetShortCode = targetStatus.ShortCode?
                .Trim()
                .ToLowerInvariant();

            if (targetShortCode == "completed")
            {
                var completionResult = await CompleteOrderAsync(
                    order,
                    cancellationToken);

                if (!completionResult.IsSuccess)
                {
                    return completionResult;
                }
            }

            order.OrderStatus = request.TargetStatusCode;

            var history = new WorkflowHistory
            {
                WorkflowType = SalesWorkflowType,
                RecordId = order.Id,
                FromStatusCode = previousStatusCode,
                ToStatusCode = request.TargetStatusCode,
                EmployeeId = employeeId,
                Note = request.Note,
                ChangeDate = DateTime.UtcNow
            };

            await _workflowHistoryRepository.AddAsync(history);

            _orderRepository.Update(order);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(
                order.Id,
                200,
                "Sipariş durumu başarıyla güncellendi.");
        }

        private async Task<Response<int>> CompleteOrderAsync(
            Order order,
            CancellationToken cancellationToken)
        {
            if (order.OrderItems == null ||
                !order.OrderItems.Any())
            {
                return Response<int>.Fail(
                    400,
                    "Sipariş tamamlanabilmesi için en az bir sipariş kalemi bulunmalıdır.");
            }

            var productSaleReason = await _parameterValueRepository
                .GetByShortCodeAsync(
                    "ReasonForEntryOrExit",
                    "URNSTS",
                    1,
                    cancellationToken);

            if (productSaleReason == null)
            {
                return Response<int>.Fail(
                    500,
                    "Ürün satış stok hareket nedeni tanımlanmamış.");
            }

            foreach (var orderItem in order.OrderItems)
            {
                if (orderItem.Quantity <= 0)
                {
                    return Response<int>.Fail(
                        400,
                        $"Sipariş kalemi miktarı geçersiz. Kalem ID: {orderItem.Id}");
                }

                if (orderItem.OrderItemWarehouses == null ||
                    !orderItem.OrderItemWarehouses.Any())
                {
                    return Response<int>.Fail(
                        400,
                        $"{orderItem.Product?.Name ?? $"Ürün #{orderItem.ProductId}"} için depo dağılımı bulunamadı.");
                }

                var allocatedQuantity = orderItem.OrderItemWarehouses.Sum(
                    allocation => allocation.Quantity);

                if (Math.Abs(allocatedQuantity - orderItem.Quantity) > 0.000001)
                {
                    return Response<int>.Fail(
                        400,
                        $"{orderItem.Product?.Name ?? $"Ürün #{orderItem.ProductId}"} için depo dağılımı sipariş miktarına eşit değildir.");
                }

                foreach (var allocation in orderItem.OrderItemWarehouses)
                {
                    if (allocation.Quantity <= 0)
                    {
                        return Response<int>.Fail(
                            400,
                            "Depodan karşılanacak ürün miktarı sıfırdan büyük olmalıdır.");
                    }

                    await _productWarehouseRepository.DecreaseStockAsync(
                        warehouseId: allocation.WarehouseId,
                        productId: orderItem.ProductId,
                        quantity: allocation.Quantity,
                        reason: productSaleReason.ParamCode,
                        cancellationToken: cancellationToken);
                }
            }

            return Response<int>.Success(order.Id, 200);
        }
    }
}

using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Payments.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Payments.Handlers
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Response<int>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IWorkflowHistoryRepository _workflowHistoryRepository;
        private readonly IProductWarehouseRepository _productWarehouseRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentCommandHandler(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IParameterValueRepository parameterValueRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IWorkflowHistoryRepository workflowHistoryRepository, IProductWarehouseRepository productWarehouseRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _parameterValueRepository = parameterValueRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _workflowHistoryRepository = workflowHistoryRepository;
            _productWarehouseRepository = productWarehouseRepository;
        }

        public async Task<Response<int>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderWithPaymentsAsync(request.OrderId, cancellationToken);

            if (order == null)
            {
                return Response<int>.Fail(404, "Ödeme yapılmak istenen sipariş bulunamadı.");
            }

            var approvedStatus = await _parameterValueRepository.GetByShortCodeAsync(
                                                "OrderStatus",
                                                "approved",
                                                1,
                                                cancellationToken);

            if (approvedStatus == null)
            {
                return Response<int>.Fail(
                    500,
                    "Sipariş durum parametreleri eksik.");
            }

            if (order.OrderStatus != approvedStatus.ParamCode)
            {
                return Response<int>.Fail(
                    400,
                    "Yalnızca onaylanmış siparişlere ödeme alınabilir.");
            }

            if (request.Currency != order.Currency)
            {
                return Response<int>.Fail(
                    400,
                    "Ödeme para birimi sipariş para birimiyle aynı olmalıdır.");
            }

            var completedPaymentStatus = await _parameterValueRepository.GetByShortCodeAsync(
                                                        "PaymentStatus",
                                                        "paid",
                                                        1,
                                                        cancellationToken);

            if (completedPaymentStatus == null)
            {
                return Response<int>.Fail(
                    500,
                    "Tamamlanmış ödeme durum parametresi tanımlanmamış.");
            }

            var payment = request.Adapt<Payment>();

            decimal totalPaidAmountBeforeThis = await _paymentRepository.GetTotalPaidAmountByOrderIdAsync(request.OrderId, cancellationToken);

            var remainingAmount = order.FinalAmount - totalPaidAmountBeforeThis;

            decimal totalPaidAmount = request.Amount + totalPaidAmountBeforeThis;

            if (request.Amount <= 0)
            {
                return Response<int>.Fail(
                    400,
                    "Ödeme tutarı sıfırdan büyük olmalıdır.");
            }

            if (remainingAmount <= 0)
            {
                return Response<int>.Fail(
                    400,
                    "Bu siparişin kalan alacağı bulunmamaktadır.");
            }

            const decimal tolerance = 0.01m;

            var isFullyPaid = order.FinalAmount - totalPaidAmount <= tolerance;

            if (totalPaidAmount > order.FinalAmount)
            {
                return Response<int>.Fail(400, $"Ödeme tutarı kalan alacağı aşamaz. " + $"Kalan alacak: {remainingAmount:N2}");
            }
            else if (isFullyPaid)
            {
                order.PaymentStatus = await _parameterValueRepository.ParamValueToParamCode("PaymentStatus", "Paid", cancellationToken); // Ödeme tamamlandı
            }
            else
            {
                order.PaymentStatus = await _parameterValueRepository.ParamValueToParamCode("PaymentStatus", "PartiallyPaid", cancellationToken); // Kısmi ödeme yapıldı
            }

            var employeeId = 0;

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _paymentRepository.AddAsync(payment);

            if (isFullyPaid)
            {
                var completionResult = await CompleteOrderAutomaticallyAsync(
                        order,
                        employeeId,
                        cancellationToken);

                if (!completionResult.IsSuccess)
                {
                    return completionResult;
                }
            }

            _orderRepository.Update(order);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(payment.Id, 201, "Ödeme başarıyla kaydedildi ve sipariş ödeme durumu güncellendi.");
        }

        private async Task<Response<int>> CompleteOrderAutomaticallyAsync(Order order, int employeeId, CancellationToken cancellationToken)
        {
            var approvedStatus = await _parameterValueRepository.GetByShortCodeAsync(
                "OrderStatus",
                "approved",
                1,
                cancellationToken);

            var completedStatus = await _parameterValueRepository.GetByShortCodeAsync(
                        "OrderStatus",
                        "completed",
                        1,
                        cancellationToken);

            if (approvedStatus == null || completedStatus == null)
            {
                return Response<int>.Fail(
                    500,
                    "Sipariş tamamlama durumları tanımlanmamış.");
            }

            if (order.OrderStatus != approvedStatus.ParamCode)
            {
                return Response<int>.Fail(
                    400,
                    "Yalnızca onaylanmış siparişler otomatik tamamlanabilir.");
            }

            var previousStatusCode =
                order.OrderStatus.Value;

            var stockResult = await CompleteOrderStockAsync(
                order,
                cancellationToken);

            if (!stockResult.IsSuccess)
            {
                return stockResult;
            }

            order.OrderStatus =
                completedStatus.ParamCode;

            var history = new WorkflowHistory
            {
                WorkflowType = 3,
                RecordId = order.Id,
                FromStatusCode = previousStatusCode,
                ToStatusCode = completedStatus.ParamCode,
                EmployeeId = employeeId,
                Note = "Siparişin kalan alacağı sıfırlandığı için sistem tarafından otomatik tamamlandı.",
                ChangeDate = DateTime.UtcNow
            };

            await _workflowHistoryRepository.AddAsync(
                history);

            return Response<int>.Success(
                order.Id,
                200);
        }

        private async Task<Response<int>> CompleteOrderStockAsync(Order order, CancellationToken cancellationToken)
        {
            if (order.IsStockProcessed)
            {
                return Response<int>.Success(
                    order.Id,
                    200);
            }

            if (order.OrderItems == null ||
                !order.OrderItems.Any())
            {
                return Response<int>.Fail(
                    400,
                    "Sipariş kalemi bulunamadı.");
            }

            var productSaleReason = await _parameterValueRepository.GetByShortCodeAsync(
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
                if (orderItem.OrderItemWarehouses == null ||
                    !orderItem.OrderItemWarehouses.Any())
                {
                    return Response<int>.Fail(
                        400,
                        $"{orderItem.Product?.Name ?? $"Ürün #{orderItem.ProductId}"} için depo dağılımı bulunamadı.");
                }

                var allocatedQuantity =
                    orderItem.OrderItemWarehouses.Sum(
                        allocation => allocation.Quantity);

                if (Math.Abs(
                        allocatedQuantity -
                        orderItem.Quantity) >
                    0.000001)
                {
                    return Response<int>.Fail(
                        400,
                        "Depo dağılımı sipariş miktarıyla eşleşmiyor.");
                }

                foreach (var allocation in
                         orderItem.OrderItemWarehouses)
                {
                    await _productWarehouseRepository
                        .DecreaseStockAsync(
                            warehouseId:
                                allocation.WarehouseId,

                            productId:
                                orderItem.ProductId,

                            quantity:
                                allocation.Quantity,

                            reason:
                                productSaleReason.ParamCode,

                            cancellationToken:
                                cancellationToken);
                }
            }

            order.IsStockProcessed = true;
            order.StockProcessedDate = DateTime.UtcNow;

            return Response<int>.Success(
                order.Id,
                200);
        }
    }
}

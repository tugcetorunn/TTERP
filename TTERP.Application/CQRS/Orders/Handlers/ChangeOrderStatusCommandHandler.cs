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
                return Response<int>.Fail(
                    400,
                    "Sipariş tamamlandı durumuna manuel olarak geçirilemez. " +
                    "Kalan alacak sıfırlandığında sistem tarafından otomatik tamamlanır.");
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
    }
}

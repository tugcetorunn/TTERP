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
    public class ChangeOrderShippingStatusCommandHandler : IRequestHandler<ChangeOrderShippingStatusCommand, Response<int>>
    {
        private const int ShippingWorkflowType = 7;
        private readonly IOrderRepository _orderRepository;
        private readonly IWorkflowService _workflowService;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IWorkflowHistoryRepository _workflowHistoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeOrderShippingStatusCommandHandler(IOrderRepository orderRepository, IWorkflowService workflowService, IParameterValueRepository parameterValueRepository, IWorkflowHistoryRepository workflowHistoryRepository, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _workflowService = workflowService;
            _parameterValueRepository = parameterValueRepository;
            _workflowHistoryRepository = workflowHistoryRepository;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(ChangeOrderShippingStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.FindAsync(request.OrderId);

            if (order == null)
            {
                return Response<int>.Fail(
                    404,
                    "Sipariş bulunamadı.");
            }

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
                    "Sipariş durum parametreleri eksik.");
            }

            var canChangeShipping = order.OrderStatus == approvedStatus.ParamCode || order.OrderStatus == completedStatus.ParamCode;

            if (!canChangeShipping)
            {
                return Response<int>.Fail(
                    400,
                    "Nakliye durumu yalnızca onaylanmış siparişlerde değiştirilebilir.");
            }

            var transition = await _workflowService.ValidateTransitionAsync(workflowType: ShippingWorkflowType,
                                                                            fromStatusCode: (int)order.ShippingStatus!,
                                                                            toStatusCode: request.TargetShippingStatusCode,
                                                                            cancellationToken: cancellationToken);

            if (transition == null)
            {
                return Response<int>.Fail(
                    400,
                    "Bu nakliye durum geçişine izin verilmiyor.");
            }

            var employeeClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(
                employeeClaim,
                out var employeeId))
            {
                return Response<int>.Fail(
                    401,
                    "Kullanıcı bilgisi alınamadı.");
            }

            var previousStatus = order.ShippingStatus;

            order.ShippingStatus = request.TargetShippingStatusCode;

            await _workflowHistoryRepository.AddAsync( new WorkflowHistory
                    {
                        WorkflowType = ShippingWorkflowType,
                        RecordId = order.Id,
                        FromStatusCode = previousStatus,
                        ToStatusCode = request.TargetShippingStatusCode,
                        EmployeeId = employeeId,
                        Note = request.Note,
                        ChangeDate = DateTime.UtcNow
                    });

            _orderRepository.Update(order);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return Response<int>.Success(
                order.Id,
                200,
                "Nakliye durumu güncellendi.");
        }
    }
}

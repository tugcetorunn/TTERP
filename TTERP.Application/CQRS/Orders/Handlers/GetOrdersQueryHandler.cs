using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Orders.Queries;
using TTERP.Application.Helpers;
using TTERP.Application.Interfaces;
using TTERP.Application.Models.DTOs.Orders;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Orders.Handlers
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, Response<IReadOnlyList<GetOrdersDTO>>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IWorkflowService _workflowService;
        public GetOrdersQueryHandler(IOrderRepository orderRepository, IParameterValueRepository parameterValueRepository, IWorkflowService workflowService)
        {
            _orderRepository = orderRepository;
            _parameterValueRepository = parameterValueRepository;
            _workflowService = workflowService;
        }

        public async Task<Response<IReadOnlyList<GetOrdersDTO>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetListWithFilterAsync(
                select: o => o.Adapt<GetOrdersDTO>(),
                where: o => o.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || o.IsActive == request.IsActive.Value),
                include: query => query.Include(order => order.Customer)
                                    .Include(order => order.Employee)
                                    .Include(order => order.OrderItems)!
                                        .ThenInclude(item => item.Product)
                                    .Include(order => order.OrderItems)!
                                        .ThenInclude(item => item.OrderItemWarehouses)!
                                            .ThenInclude(allocation => allocation.Warehouse)
                                    .Include(order => order.Payments)!);

            var statusValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                "OrderStatus",
                                1,
                                cancellationToken);

            var statusDictionary = statusValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);

            var statusShortCodeDictionary = statusValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ShortCode);

            var paymentValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                "PaymentStatus",
                                1,
                                cancellationToken);

            var paymentDictionary = paymentValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);

            var shippingValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                "ShippingStatus",
                                1,
                                cancellationToken);

            var shippingDictionary = shippingValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);

            var currencyValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                "Currency",
                                1,
                                cancellationToken);

            var currencyDictionary = currencyValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);

            foreach (var order in orders)
            {
                order.OrderStatusName = statusDictionary.GetValueOrDefault(order.OrderStatus);
                order.PaymentStatusName = paymentDictionary.GetValueOrDefault(order.PaymentStatus);
                order.ShippingStatusName = shippingDictionary.GetValueOrDefault(order.ShippingStatus);
                order.CurrencyName = currencyDictionary.GetValueOrDefault(order.Currency);

                order.AllowedTransitions = await _workflowService.GetAllowedTransitionsAsync(
                    workflowType: 3,
                    currentStatusCode: order.OrderStatus,
                    cancellationToken: cancellationToken);

                order.Actions = WorkflowActionHelper.CreateActions(
                    workflowType: 3,
                    statusShortCode: statusShortCodeDictionary.GetValueOrDefault(order.OrderStatus));

            }

            return Response<IReadOnlyList<GetOrdersDTO>>.Success(orders.ToList());
        }
    }
}

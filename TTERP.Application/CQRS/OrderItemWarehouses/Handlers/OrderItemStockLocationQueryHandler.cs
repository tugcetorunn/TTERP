using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.OrderItemWarehouses.Queries;
using TTERP.Application.Models.DTOs.MaterialWarehouses;
using TTERP.Application.Models.DTOs.OrderItemWarehouses;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.OrderItemWarehouses.Handlers
{
    public class OrderItemStockLocationQueryHandler : IRequestHandler<OrderItemStockLocationQuery, Response<IReadOnlyList<OrderItemStockLocationDTO>>>
    {
        private readonly IOrderItemWarehouseRepository _orderItemWarehouseRepository;

        public OrderItemStockLocationQueryHandler(IOrderItemWarehouseRepository orderItemWarehouseRepository)
        {
            _orderItemWarehouseRepository = orderItemWarehouseRepository;
        }

        public async Task<Response<IReadOnlyList<OrderItemStockLocationDTO>>> Handle(OrderItemStockLocationQuery request, CancellationToken cancellationToken)
        {
            var orderItemStock = await _orderItemWarehouseRepository.GetListWithFilterAsync(
                oi => oi.Adapt<OrderItemStockLocationDTO>(),
                oi => oi.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || oi.IsActive == request.IsActive.Value)
                && oi.OrderItemId == request.OrderItemId);

            return Response<IReadOnlyList<OrderItemStockLocationDTO>>.Success(orderItemStock.ToList());
        }
    }
}

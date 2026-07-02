using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Orders.Queries;
using TTERP.Application.Models.DTOs.Invoices;
using TTERP.Application.Models.DTOs.Orders;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Orders.Handlers
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, Response<IReadOnlyList<GetOrdersDTO>>>
    {
        private readonly IOrderRepository _orderRepository;
        public GetOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Response<IReadOnlyList<GetOrdersDTO>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetListWithFilterAsync(
                select: o => o.Adapt<GetOrdersDTO>(),
                where: o => o.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || o.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetOrdersDTO>>.Success(orders.ToList());
        }
    }
}

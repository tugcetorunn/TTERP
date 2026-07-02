using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.OrderItems.Queries;
using TTERP.Application.Models.DTOs.Invoices;
using TTERP.Application.Models.DTOs.OrderItems;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.OrderItems.Handlers
{
    public class GetOrderItemsQueryHandler : IRequestHandler<GetOrderItemsQuery, Response<IReadOnlyList<GetOrderItemsDTO>>>
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public GetOrderItemsQueryHandler(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<Response<IReadOnlyList<GetOrderItemsDTO>>> Handle(GetOrderItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _orderItemRepository.GetListWithFilterAsync(
                select: oi => oi.Adapt<GetOrderItemsDTO>(),
                where: oi => oi.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || oi.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetOrderItemsDTO>>.Success(items.ToList());
        }
    }
}

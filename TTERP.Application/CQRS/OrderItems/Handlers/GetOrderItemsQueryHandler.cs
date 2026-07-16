using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.OrderItems.Queries;
using TTERP.Application.Models.DTOs.Invoices;
using TTERP.Application.Models.DTOs.OrderItems;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.OrderItems.Handlers
{
    public class GetOrderItemsQueryHandler : IRequestHandler<GetOrderItemsQuery, Response<IReadOnlyList<GetOrderItemsDTO>>>
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IParameterValueRepository _parameterValueRepository;

        public GetOrderItemsQueryHandler(IOrderItemRepository orderItemRepository, IParameterValueRepository parameterValueRepository)
        {
            _orderItemRepository = orderItemRepository;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<IReadOnlyList<GetOrderItemsDTO>>> Handle(GetOrderItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _orderItemRepository.GetListWithFilterAsync(
                select: oi => oi.Adapt<GetOrderItemsDTO>(),
                where: oi => oi.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || oi.IsActive == request.IsActive.Value),
                include: oi => oi.Include(oi => oi.Product)!
                                 .Include(oi => oi.Order)!);

            var currencyValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                "Currency",
                                1,
                                cancellationToken);

            var currencyDictionary = currencyValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);

            foreach (var item in items)
            {
                item.CurrencyName = currencyDictionary.GetValueOrDefault(item.Currency)!;
            }

            return Response<IReadOnlyList<GetOrderItemsDTO>>.Success(items.ToList());
        }
    }
}

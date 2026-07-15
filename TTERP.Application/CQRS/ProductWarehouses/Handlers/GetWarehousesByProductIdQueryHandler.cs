using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Queries;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductWarehouses.Handlers
{
    public class GetWarehousesByProductIdQueryHandler : IRequestHandler<GetWarehousesByProductIdQuery, Response<IReadOnlyList<GetProductToWarehousesDTO>>>
    {
        private readonly IProductWarehouseRepository _productWarehouseRepository;

        public GetWarehousesByProductIdQueryHandler(IProductWarehouseRepository productWarehouseRepository)
        {
            _productWarehouseRepository = productWarehouseRepository;
        }

        public async Task<Response<IReadOnlyList<GetProductToWarehousesDTO>>> Handle(GetWarehousesByProductIdQuery request, CancellationToken cancellationToken)
        {
            var stocks = await _productWarehouseRepository.GetProductsStockAsync(
                    productId: request.ProductId,
                    warehouseId: null,
                    cancellationToken: cancellationToken);

            var result = stocks.Select(stock => new GetProductToWarehousesDTO
            {
                WarehouseId = stock.WarehouseId,
                WarehouseName = stock.WarehouseName,
                WarehouseCode = stock.WarehouseCode,
                Quantity = stock.TotalQuantity,
                IsActive = stock.IsActive,
                IsDeleted = stock.IsDeleted
            }).ToList();

            return Response<IReadOnlyList<GetProductToWarehousesDTO>>.Success(result);
        }
    }
}

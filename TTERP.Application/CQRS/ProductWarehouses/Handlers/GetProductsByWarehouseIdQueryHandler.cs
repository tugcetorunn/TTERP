using MediatR;
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
    public class GetProductsByWarehouseIdQueryHandler : IRequestHandler<GetProductsByWarehouseIdQuery, Response<IReadOnlyList<GetWarehouseToProductsDTO>>>
    {
        private readonly IProductWarehouseRepository _productWarehouseRepository;

        public GetProductsByWarehouseIdQueryHandler(IProductWarehouseRepository productWarehouseRepository)
        {
            _productWarehouseRepository = productWarehouseRepository;
        }

        public async Task<Response<IReadOnlyList<GetWarehouseToProductsDTO>>> Handle(GetProductsByWarehouseIdQuery request, CancellationToken cancellationToken)
        {
            var stocks = await _productWarehouseRepository.GetProductsStockAsync(
                    warehouseId: request.WarehouseId,
                    productId: null,
                    cancellationToken: cancellationToken);

            var result = stocks.Select(stock => new GetWarehouseToProductsDTO
            {
                ProductId = stock.ProductId,
                ProductName = stock.ProductName,
                ProductCode = stock.ProductCode,
                Quantity = stock.TotalQuantity,
                IsActive = stock.IsActive,
                IsDeleted = stock.IsDeleted
            }).ToList();

            return Response<IReadOnlyList<GetWarehouseToProductsDTO>>.Success(result);
        }
    }
}

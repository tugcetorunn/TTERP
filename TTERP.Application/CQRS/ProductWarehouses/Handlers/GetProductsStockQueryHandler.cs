using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Queries;
using TTERP.Application.Models.DTOs.MaterialWarehouses;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductWarehouses.Handlers
{
    public class GetProductsStockQueryHandler : IRequestHandler<GetProductsStockQuery, Response<IReadOnlyList<GetProductsStockDTO>>>
    {
        private readonly IProductWarehouseRepository _productWarehouseRepository;

        public GetProductsStockQueryHandler(IProductWarehouseRepository productWarehouseRepository)
        {
            _productWarehouseRepository = productWarehouseRepository;
        }

        public async Task<Response<IReadOnlyList<GetProductsStockDTO>>> Handle(GetProductsStockQuery request, CancellationToken cancellationToken)
        {
            var stocks = await _productWarehouseRepository.GetProductsStockAsync(request.ProductId, request.WarehouseId, cancellationToken);

            var response = stocks.Adapt<List<GetProductsStockDTO>>();

            return Response<IReadOnlyList<GetProductsStockDTO>>.Success(response, 200);
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.MaterialWarehouses.Queries;
using TTERP.Application.CQRS.ProductWarehouses.Commands;
using TTERP.Application.CQRS.ProductWarehouses.Queries;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWarehousesController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public ProductWarehousesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetList))]
        public async Task<IActionResult> GetList(int? productId, int? warehouseId, bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetProductWarehousesQuery(productId, warehouseId, isActive, isDeleted));

            return CreateActionResultInstance(result);
        }

        [HttpGet(nameof(GetStockList))]
        public async Task<IActionResult> GetStockList(int? productId, int? warehouseId, bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetProductsStockQuery(productId, warehouseId, isActive, isDeleted));

            return CreateActionResultInstance(result);
        }

        [HttpGet(nameof(GetWarehousesByProductId))]
        public async Task<IActionResult> GetWarehousesByProductId(int productId, bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetWarehousesByProductIdQuery(productId, isActive, isDeleted));
            return CreateActionResultInstance(result);
        }

        [HttpGet(nameof(GetProductsByWarehouseId))]
        public async Task<IActionResult> GetProductsByWarehouseId(int warehouseId, bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetProductsByWarehouseIdQuery(warehouseId, isActive, isDeleted));
            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(Create))]
        public async Task<IActionResult> Create(CreateProductWarehouseCommand command)
        {
            var result = await _mediator.Send(command);

            return CreateActionResultInstance(result);
        }
    }
}

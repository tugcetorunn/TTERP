using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetList()
        {
            var result = await _mediator.Send(new GetInventoryListQuery());

            return CreateActionResultInstance(result);
        }

        [HttpGet(nameof(GetWarehousesByProductId))]
        public async Task<IActionResult> GetWarehousesByProductId(int productId)
        {
            var result = await _mediator.Send(new GetWarehousesByProductIdQuery { ProductId = productId });
            return CreateActionResultInstance(result);
        }
    }
}

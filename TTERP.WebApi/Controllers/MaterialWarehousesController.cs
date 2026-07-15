using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.MaterialWarehouses.Commands;
using TTERP.Application.CQRS.MaterialWarehouses.Queries;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialWarehousesController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public MaterialWarehousesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetList))]
        public async Task<IActionResult> GetList(int? materialId, int? warehouseId, bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetMaterialWarehousesQuery(materialId, warehouseId, isActive, isDeleted));

            return CreateActionResultInstance(result);
        }

        [HttpGet(nameof(GetStockList))]
        public async Task<IActionResult> GetStockList(int? materialId, int? warehouseId, bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetMaterialsStockQuery(materialId, warehouseId, isActive, isDeleted));

            return CreateActionResultInstance(result);
        }

        [HttpGet(nameof(GetTimeline))]
        public async Task<IActionResult> GetTimeline(int? materialId, int? warehouseId, bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetMaterialStockTimelineQuery(materialId, warehouseId, isActive, isDeleted));

            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(Create))]
        public async Task<IActionResult> Create(CreateMaterialWarehouseCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }
    }
}

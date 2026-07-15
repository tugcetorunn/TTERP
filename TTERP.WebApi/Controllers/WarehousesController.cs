using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.Warehouses.Commands;
using TTERP.Application.CQRS.Warehouses.Queries;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public WarehousesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetList))]
        public async Task<IActionResult> GetList(bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetWarehousesQuery(isActive, isDeleted));

            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(Create))]
        public async Task<IActionResult> Create(CreateWarehouseCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }
    }
}

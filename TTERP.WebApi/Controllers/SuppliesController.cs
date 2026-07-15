using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.Supplies.Commands;
using TTERP.Application.CQRS.Supplies.Queries;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliesController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public SuppliesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetList))]
        public async Task<IActionResult> GetList(bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetSuppliesQuery(isActive, isDeleted));

            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(Create))]
        public async Task<IActionResult> Create(CreateSupplyCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }

        [HttpPost("CompleteSupply")]
        public async Task<IActionResult> Complete(CompleteSupplyCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(ChangeStatus))]
        public async Task<IActionResult> ChangeStatus(ChangeSupplyStatusCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }
    }
}

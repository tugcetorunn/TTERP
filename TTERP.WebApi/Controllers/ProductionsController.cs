using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.Productions.Commands;
using TTERP.Application.CQRS.Productions.Queries;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionsController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public ProductionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetList))]
        public async Task<IActionResult> GetList(bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetProductionsQuery(isActive, isDeleted));

            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(Plan))]
        public async Task<IActionResult> Plan(PlanProductionCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(Start))]
        public async Task<IActionResult> Start(StartProductionCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(Complete))]
        public async Task<IActionResult> Complete(CompleteProductionCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(ChangeStatus))]
        public async Task<IActionResult> ChangeStatus(ChangeProductionStatusCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }
    }
}

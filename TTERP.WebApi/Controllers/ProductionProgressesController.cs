using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.ProductionProgresses.Commands;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionProgressesController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public ProductionProgressesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(nameof(AddProgress))]
        public async Task<IActionResult> AddProgress(AddProductionProgressCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }
    }
}

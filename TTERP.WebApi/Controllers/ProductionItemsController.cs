using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.ProductionItems.Queries;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionItemsController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public ProductionItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetList))]
        public async Task<IActionResult> GetList(int productionId, bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetProductionItemsQuery(productionId, isActive,isDeleted));

            return CreateActionResultInstance(result);
        }
    }
}

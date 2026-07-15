using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.SupplyItems.Commands;
using TTERP.Application.CQRS.SupplyItems.Queries;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyItemsController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public SupplyItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetList))]
        public async Task<IActionResult> GetList(int supplyId, bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetSupplyItemsQuery(supplyId, isActive, isDeleted));

            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(AddItem))]
        public async Task<IActionResult> AddItem(AddSupplyItemCommand command)
        {
            var result = await _mediator.Send(command);

            return CreateActionResultInstance(result);
        }

        //[HttpGet(Name = "GetById")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var result = await _mediator.Send(new GetAnnouncementByIdQuery { Id = id });
        //    return CreateActionResultInstance(result);
        //}
    }
}

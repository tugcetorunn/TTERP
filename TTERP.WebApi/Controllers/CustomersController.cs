using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.Customers.Commands;
using TTERP.Application.CQRS.Customers.Queries;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetList))]
        public async Task<IActionResult> GetList(bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetCustomersQuery(isActive, isDeleted));

            return CreateActionResultInstance(result);
        }

        //[HttpGet(Name = "GetById")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var result = await _mediator.Send(new GetAnnouncementByIdQuery { Id = id });
        //    return CreateActionResultInstance(result);
        //}

        [HttpPost(nameof(Create))]
        public async Task<IActionResult> Create(CreateCustomerCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }
    }
}

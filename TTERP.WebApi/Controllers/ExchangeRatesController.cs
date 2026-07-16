using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.ExchangeRates.Queries;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExchangeRatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetCurrentRates))]
        public async Task<IActionResult> GetCurrentRates(
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(
                new GetCurrentExchangeRatesQuery(),
                cancellationToken);

            return Ok(response);
        }
    }
}

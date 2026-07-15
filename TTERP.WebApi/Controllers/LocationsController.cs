using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.Cities.Queries;
using TTERP.Application.CQRS.Countries.Queries;
using TTERP.Application.CQRS.Districts.Queries;
using TTERP.Application.CQRS.Neighborhoods.Queries;
using TTERP.Application.CQRS.Towns.Queries;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public LocationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Countries")]
        public async Task<IActionResult> GetCountries(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetCountriesQuery(), cancellationToken);

            return CreateActionResultInstance(response);
        }

        [HttpGet("Countries/{countryId:int}/Cities")]
        public async Task<IActionResult> GetCities(int countryId, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetCitiesByCountryIdQuery(countryId), cancellationToken);

            return CreateActionResultInstance(response);
        }

        [HttpGet("Cities/{cityId:int}/Towns")]
        public async Task<IActionResult> GetTowns(int cityId, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetTownsByCityIdQuery(cityId), cancellationToken);

            return CreateActionResultInstance(response);
        }

        [HttpGet("Towns/{townId:int}/Districts")]
        public async Task<IActionResult> GetDistricts(int townId, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetDistrictsByTownIdQuery(townId), cancellationToken);

            return CreateActionResultInstance(response);
        }

        [HttpGet("Districts/{districtId:int}/Neighborhoods")]
        public async Task<IActionResult> GetNeighborhoods(int districtId, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetNeighborhoodsByDistrictIdQuery(districtId), cancellationToken);

            return CreateActionResultInstance(response);
        }
    }
}

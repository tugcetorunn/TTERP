using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.ParameterValues.Commands;
using TTERP.Application.CQRS.ParameterValues.Queries;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParameterValuesController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public ParameterValuesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetList))]
        public async Task<IActionResult> GetList(bool? isActive, bool? isDeleted, int? languageId)
        {
            var result = await _mediator.Send(new GetParameterValuesQuery(isActive, isDeleted, languageId));

            return CreateActionResultInstance(result);
        }

        [HttpGet(nameof(GetByParamType))]
        public async Task<IActionResult> GetByParamType(string paramType, bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetParameterValuesByTypeQuery(paramType, isActive, isDeleted));

            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(Create))]
        public async Task<IActionResult> Create(CreateParameterValueExceptDefinitionCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }
    }
}

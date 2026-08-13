using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.Permissions.Queries;
using TTERP.Application.CQRS.Roles.Commands;
using TTERP.Application.CQRS.Roles.Queries;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetList))]
        public async Task<IActionResult> GetList(bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetRolesQuery(isActive, isDeleted));
            return CreateActionResultInstance(result);
        }

        [HttpGet(nameof(GetDetail))]
        public async Task<IActionResult> GetDetail(int id)
        {
            var result = await _mediator.Send(new GetRoleDetailQuery(id));
            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(Create))]
        public async Task<IActionResult> Create(CreateRoleCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }

        [HttpGet(nameof(GetPermissions))]
        public async Task<IActionResult> GetPermissions(bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetPermissionsQuery(isActive, isDeleted));
            return CreateActionResultInstance(result);
        }

        [HttpPut(nameof(UpdatePermissions))]
        public async Task<IActionResult> UpdatePermissions(UpdateRolePermissionsCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }
    }
}

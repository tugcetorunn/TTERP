using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTERP.Application.CQRS.TaskAssignments.Commands;
using TTERP.Application.CQRS.TaskAssignments.Queries;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskAssignmentsController : CustomBaseController
    {
        private readonly IMediator _mediator;

        public TaskAssignmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetList))]
        public async Task<IActionResult> GetList(int taskId, bool? isActive, bool? isDeleted)
        {
            var result = await _mediator.Send(new GetTaskAssignmentsQuery(taskId, isActive, isDeleted));

            return CreateActionResultInstance(result);
        }

        //[HttpGet(Name = "GetById")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var result = await _mediator.Send(new GetAnnouncementByIdQuery { Id = id });
        //    return CreateActionResultInstance(result);
        //}

        [HttpPost(nameof(Create))]
        public async Task<IActionResult> Create(CreateTaskAssignmentCommand command)
        {
            var result = await _mediator.Send(command);
            return CreateActionResultInstance(result);
        }
    }
}

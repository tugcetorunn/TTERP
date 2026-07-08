using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Tasks.Queries;
using TTERP.Application.Models.DTOs.Tasks;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Tasks.Handlers
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, Response<IReadOnlyList<GetTasksDTO>>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public GetTasksQueryHandler(ITaskRepository taskRepository, IEmployeeRepository employeeRepository, IParameterValueRepository parameterValueRepository, IHttpContextAccessor contextAccessor, ITaskAssignmentRepository taskAssignmentRepository)
        {
            _taskRepository = taskRepository;
            _employeeRepository = employeeRepository;
            _parameterValueRepository = parameterValueRepository;
            _contextAccessor = contextAccessor;
            _taskAssignmentRepository = taskAssignmentRepository;
        }

        public async Task<Response<IReadOnlyList<GetTasksDTO>>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            List<int> targetEmployeeIds = new List<int>();

            var currentUserIdValue = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int currentUserId = int.Parse(currentUserIdValue!);

            bool isManager = await _employeeRepository.IsManagerAsync(currentUserId);

            if (isManager)
            {
                var teamEmployeeIds = await _employeeRepository.GetTeamEmployeeIdsAsync(currentUserId);
                targetEmployeeIds.AddRange(teamEmployeeIds);
                targetEmployeeIds.Add(currentUserId);
            }
            else
            {
                targetEmployeeIds.Add(currentUserId);
            }

            var roleCode = await _parameterValueRepository.ParamValueToParamCode("TaskAssignmentRole", "Responsible", cancellationToken);

            var tasks = await _taskRepository.GetTasksByAssignmentRoleAsync(roleCode, targetEmployeeIds, cancellationToken);

            //var tasks = await _taskRepository.GetListWithFilterAsync(
            //    t => t.Adapt<GetTasksDTO>(),
            //    t => t.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || t.IsActive == request.IsActive.Value) &&
            //    (
            //        (t.AssignedToEmployeeId.HasValue && targetEmployeeIds.Contains(t.AssignedToEmployeeId.Value))
            //    ));

            return Response<IReadOnlyList<GetTasksDTO>>.Success(tasks.Adapt<IReadOnlyList<GetTasksDTO>>());
        }
    }
}

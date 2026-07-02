using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Queries;
using TTERP.Application.CQRS.Tasks.Queries;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.Tasks;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Tasks.Handlers
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, Response<IReadOnlyList<GetTasksDTO>>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public GetTasksQueryHandler(ITaskRepository taskRepository, IEmployeeRepository employeeRepository)
        {
            _taskRepository = taskRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<Response<IReadOnlyList<GetTasksDTO>>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            List<int> targetEmployeeIds = new List<int>();

            bool isManager = await _employeeRepository.IsManagerAsync(request.CurrentUserId);

            if (isManager)
            {
                var teamEmployeeIds = await _employeeRepository.GetTeamEmployeeIdsAsync(request.CurrentUserId);
                targetEmployeeIds.AddRange(teamEmployeeIds);
                targetEmployeeIds.Add(request.CurrentUserId);
            }
            else
            {
                targetEmployeeIds.Add(request.CurrentUserId);
            }

            var tasks = await _taskRepository.GetListWithFilterAsync(
                t => t.Adapt<GetTasksDTO>(),
                t => t.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || t.IsActive == request.IsActive.Value) &&
                (
                    (t.AssignedToEmployeeId.HasValue && targetEmployeeIds.Contains(t.AssignedToEmployeeId.Value))
                ));

            return Response<IReadOnlyList<GetTasksDTO>>.Success(tasks.ToList());
        }
    }
}

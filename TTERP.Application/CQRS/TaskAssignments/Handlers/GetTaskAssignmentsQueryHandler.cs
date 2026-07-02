using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Queries;
using TTERP.Application.CQRS.TaskAssignments.Queries;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.TaskAssignments;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.TaskAssignments.Handlers
{
    public class GetTaskAssignmentsQueryHandler : IRequestHandler<GetTaskAssignmentsQuery, Response<IReadOnlyList<GetTaskAssignmentsDTO>>>
    {
        private readonly ITaskAssignmentRepository _taskAssignmentRepository;

        public GetTaskAssignmentsQueryHandler(ITaskAssignmentRepository taskAssignmentRepository)
        {
            _taskAssignmentRepository = taskAssignmentRepository;
        }

        public async Task<Response<IReadOnlyList<GetTaskAssignmentsDTO>>> Handle(GetTaskAssignmentsQuery request, CancellationToken cancellationToken)
        {
            var assignments = await _taskAssignmentRepository.GetListWithFilterAsync(
                ta => ta.Adapt<GetTaskAssignmentsDTO>(),
                ta => ta.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || ta.IsActive == request.IsActive.Value) && ta.TaskId == request.TaskId);

            return Response<IReadOnlyList<GetTaskAssignmentsDTO>>.Success(assignments.ToList());
        }
    }
}

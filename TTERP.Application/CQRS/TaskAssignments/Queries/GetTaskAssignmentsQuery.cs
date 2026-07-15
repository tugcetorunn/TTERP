using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.TaskAssignments;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.TaskAssignments.Queries
{
    public class GetTaskAssignmentsQuery : IRequest<Response<IReadOnlyList<GetTaskAssignmentsDTO>>>
    {
        public int TaskId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetTaskAssignmentsQuery(int taskId, bool? isActive, bool? isDeleted)
        {
            TaskId = taskId;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

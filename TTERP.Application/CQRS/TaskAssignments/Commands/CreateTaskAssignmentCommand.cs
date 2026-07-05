using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.TaskAssignments.Commands
{
    public class CreateTaskAssignmentCommand : IRequest<Response<int>>
    {
        public int TaskId { get; set; }
        public int EmployeeId { get; set; }
        public int Role { get; set; }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.TaskAssignments.Commands;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Tasks.Commands
{
    public class CreateTaskCommand : IRequest<Response<int>>
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int TaskType { get; set; }
        public int? CustomerId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? MaterialId { get; set; }
        public DateTime DueDate { get; set; }
        public int? Priority { get; set; }
        public List<CreateTaskAssignmentCommand>? Assignments { get; set; }
    }
}

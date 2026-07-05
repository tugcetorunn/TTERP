using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Teams.Commands
{
    public class CreateTeamCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<int>? MemberIds { get; set; } = new List<int>();
        public List<int>? ManagerIds { get; set; } = new List<int>();
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.TeamManagers.Commands
{
    public class CreateTeamManagerCommand : IRequest<Response<int>>
    {
        public int TeamId { get; set; }
        public int ManagerId { get; set; }
    }
}

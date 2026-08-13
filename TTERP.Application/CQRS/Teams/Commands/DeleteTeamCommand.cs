using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Teams.Commands
{
    public class DeleteTeamCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteTeamCommand(int id)
        {
            Id = id;
        }
    }
}

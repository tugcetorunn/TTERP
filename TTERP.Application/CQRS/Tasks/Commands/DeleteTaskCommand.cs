using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Tasks.Commands
{
    public class DeleteTaskCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteTaskCommand(int id)
        {
            Id = id;
        }
    }
}

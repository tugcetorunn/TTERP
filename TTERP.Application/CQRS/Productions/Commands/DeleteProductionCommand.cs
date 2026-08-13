using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Productions.Commands
{
    public class DeleteProductionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteProductionCommand(int id)
        {
            Id = id;
        }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductionProgresses.Commands
{
    public class DeleteProductionProgressCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteProductionProgressCommand(int id)
        {
            Id = id;
        }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Supplies.Commands
{
    public class DeleteSupplyCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteSupplyCommand(int id)
        {
            Id = id;
        }
    }
}

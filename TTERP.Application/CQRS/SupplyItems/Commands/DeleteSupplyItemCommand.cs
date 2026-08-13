using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.SupplyItems.Commands
{
    public class DeleteSupplyItemCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteSupplyItemCommand(int id)
        {
            Id = id;
        }
    }
}

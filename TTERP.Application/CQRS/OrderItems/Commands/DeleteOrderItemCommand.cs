using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.OrderItems.Commands
{
    public class DeleteOrderItemCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteOrderItemCommand(int id)
        {
            Id = id;
        }
    }
}

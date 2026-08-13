using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Warehouses.Commands
{
    public class DeleteWarehouseCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteWarehouseCommand(int id)
        {
            Id = id;
        }
    }
}

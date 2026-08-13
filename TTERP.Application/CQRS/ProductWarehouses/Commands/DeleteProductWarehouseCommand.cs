using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductWarehouses.Commands
{
    public class DeleteProductWarehouseCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteProductWarehouseCommand(int id)
        {
            Id = id;
        }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Suppliers.Commands
{
    public class DeleteSupplierCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteSupplierCommand(int id)
        {
            Id = id;
        }
    }
}

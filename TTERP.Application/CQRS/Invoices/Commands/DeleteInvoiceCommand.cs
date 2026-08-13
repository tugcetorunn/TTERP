using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Invoices.Commands
{
    public class DeleteInvoiceCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteInvoiceCommand(int id)
        {
            Id = id;
        }
    }
}

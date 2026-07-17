using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Invoices.Commands
{
    public class CreateInvoiceCommand : IRequest<Response<int>>
    {
        public int OrderId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int Currency { get; set; }
    }
}

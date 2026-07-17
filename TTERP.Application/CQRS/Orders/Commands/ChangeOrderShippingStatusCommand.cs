using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Orders.Commands
{
    public class ChangeOrderShippingStatusCommand : IRequest<Response<int>>
    {
        public int OrderId { get; set; }
        public int TargetShippingStatusCode { get; set; }
        public string? Note { get; set; }
    }
}

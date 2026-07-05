using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Supplies.Commands
{
    public class CompleteSupplyCommand : IRequest<Response<int>>
    {
        public int SupplyId { get; set; }
    }
}

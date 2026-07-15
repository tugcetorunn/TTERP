using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Supplies.Commands
{
    public class ChangeSupplyStatusCommand : IRequest<Response<int>>
    {
        public int SupplyId { get; set; }
        public int TargetStatusCode { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Note { get; set; }
    }
}

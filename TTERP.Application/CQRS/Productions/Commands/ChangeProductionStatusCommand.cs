using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductionItems.Commands;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Productions.Commands
{
    public class ChangeProductionStatusCommand : IRequest<Response<int>>
    {
        public int ProductionId { get; set; }
        public int TargetStatusCode { get; set; }
        public double? ActualQuantity { get; set; }
        public List<CompleteProductionItemCommand>? ProductionItems { get; set; }
        public string? Note { get; set; }
    }
}

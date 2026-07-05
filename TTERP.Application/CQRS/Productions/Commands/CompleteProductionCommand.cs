using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductionItems.Commands;
using TTERP.Application.Models.DTOs.ProductionItems;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Productions.Commands
{
    public class CompleteProductionCommand : IRequest<Response<int>>
    {
        public int ProductionId { get; set; }
        public double ActualQuantity { get; set; }
        public List<CompleteProductionItemCommand>? CompleteProductionItems { get; set; }
    }
}

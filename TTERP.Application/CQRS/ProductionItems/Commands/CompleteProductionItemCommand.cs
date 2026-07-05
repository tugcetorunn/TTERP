using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductionItems.Commands
{
    public class CompleteProductionItemCommand : IRequest<Response<int>>
    {
        public int ProductionItemId { get; set; }
        public double ActualQuantity { get; set; }
        public double ScrapQuantity { get; set; }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductionItems.Commands
{
    public class PlanProductionItemCommand
    {
        public int MaterialId { get; set; }
        public int SourceWarehouseId { get; set; }
        public double PlannedQuantity { get; set; }
    }
}

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
    public class PlanProductionCommand : IRequest<Response<int>>
    {
        public int ProductId { get; set; }
        public double PlannedQuantity { get; set; }
        public int TargetWarehouseId { get; set; }
        public DateTime ProductionDate { get; set; }
        public List<PlanProductionItemCommand>? ProductionItems { get; set; }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ProductionProgresses.Commands
{
    public class AddProductionProgressCommand : IRequest<Response<int>>
    {
        public int ProductionId { get; set; }
        public double ProducedQuantity { get; set; }
        public string? Note { get; set; }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.SupplyItems.Commands;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Supplies.Commands
{
    public class CreateSupplyCommand : IRequest<Response<int>>
    {
        public decimal TotalAmount { get; set; }
        public DateTime SupplyDate { get; set; }
        public string DocumentNumber { get; set; }
        public int? SupplyStatus { get; set; }
        public int? EmployeeId { get; set; }
        public int? SupplierId { get; set; }
        public ICollection<CreateSupplyItemCommand>? SupplyItems { get; set; }
    }
}

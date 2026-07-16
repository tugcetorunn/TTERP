using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Invoices;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Invoices.Queries
{
    public class GetInvoicesQuery : IRequest<Response<IReadOnlyList<GetInvoicesDTO>>>
    {
        public int? OrderId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetInvoicesQuery(int? orderId, bool? isActive, bool? isDeleted)
        {
            OrderId = orderId;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

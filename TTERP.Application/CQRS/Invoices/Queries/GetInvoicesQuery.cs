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
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetInvoicesQuery(bool? isActive, bool? isDeleted)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

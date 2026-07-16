using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Payments;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Payments.Queries
{
    public class GetPaymentsQuery : IRequest<Response<IReadOnlyList<GetPaymentsDTO>>>
    {
        public int? OrderId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetPaymentsQuery(int? orderId, bool? isActive, bool? isDeleted)
        {
            OrderId = orderId;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

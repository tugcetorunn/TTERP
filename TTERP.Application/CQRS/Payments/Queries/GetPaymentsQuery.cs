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
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

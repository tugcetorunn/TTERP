using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.Supplies;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Supplies.Queries
{
    public class GetSuppliesQuery : IRequest<Response<IReadOnlyList<GetSuppliesDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

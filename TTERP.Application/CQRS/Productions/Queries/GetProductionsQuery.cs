using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Productions;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Productions.Queries
{
    public class GetProductionsQuery : IRequest<Response<IReadOnlyList<GetProductionsDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

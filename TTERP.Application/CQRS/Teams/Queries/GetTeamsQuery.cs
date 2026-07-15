using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.Teams;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Teams.Queries
{
    public class GetTeamsQuery : IRequest<Response<IReadOnlyList<GetTeamsDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetTeamsQuery(bool? isActive, bool? isDeleted)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

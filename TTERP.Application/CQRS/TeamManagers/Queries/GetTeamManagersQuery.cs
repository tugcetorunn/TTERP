using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.TeamManagers;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.TeamManagers.Queries
{
    public class GetTeamManagersQuery : IRequest<Response<IReadOnlyList<GetTeamManagersDTO>>>
    {
        public int? TeamId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetTeamManagersQuery(int? teamId, bool? isActive, bool? isDeleted)
        {
            TeamId = teamId;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

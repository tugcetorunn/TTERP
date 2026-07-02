using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Queries;
using TTERP.Application.CQRS.TeamManagers.Queries;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.TeamManagers;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.TeamManagers.Handlers
{
    public class GetTeamManagersQueryHandler : IRequestHandler<GetTeamManagersQuery, Response<IReadOnlyList<GetTeamManagersDTO>>>
    {
        private readonly ITeamManagerRepository _managerRepository;

        public GetTeamManagersQueryHandler(ITeamManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }

        public async Task<Response<IReadOnlyList<GetTeamManagersDTO>>> Handle(GetTeamManagersQuery request, CancellationToken cancellationToken)
        {
            var managers = await _managerRepository.GetListWithFilterAsync(
                tm => tm.Adapt<GetTeamManagersDTO>(),
                tm => tm.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || tm.IsActive == request.IsActive.Value) &&
                (
                    !request.TeamId.HasValue || (tm.TeamId == request.TeamId.Value)
                ));

            return Response<IReadOnlyList<GetTeamManagersDTO>>.Success(managers.ToList());
        }
    }
}

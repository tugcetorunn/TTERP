using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Queries;
using TTERP.Application.CQRS.Teams.Queries;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.Teams;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Teams.Handlers
{
    public class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, Response<IReadOnlyList<GetTeamsDTO>>>
    {
        private readonly ITeamRepository _teamRepository;

        public GetTeamsQueryHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<Response<IReadOnlyList<GetTeamsDTO>>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        {
            var teams = await _teamRepository.GetListWithFilterAsync(
                t => t.Adapt<GetTeamsDTO>(),
                t => t.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || t.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetTeamsDTO>>.Success(teams.ToList());
        }
    }
}

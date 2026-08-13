using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Roles.Queries;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Models;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Roles.Handlers
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Response<IReadOnlyList<GetRolesDTO>>>
    {
        private readonly IRoleRepository _roleRepository;

        public GetRolesQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Response<IReadOnlyList<GetRolesDTO>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetRolesWithCountsAsync(request.IsActive, request.IsDeleted, cancellationToken);

            return Response<IReadOnlyList<GetRolesDTO>>.Success(roles.ToList());
        }
    }
}

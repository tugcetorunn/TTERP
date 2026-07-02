using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Queries;
using TTERP.Application.CQRS.Roles.Queries;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.Roles;
using TTERP.Domain.Interfaces;
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
            var roles = await _roleRepository.GetListWithFilterAsync(
                r => r.Adapt<GetRolesDTO>(),
                r => r.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || r.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetRolesDTO>>.Success(roles.ToList());
        }
    }
}

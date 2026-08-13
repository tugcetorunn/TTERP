using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Permissions.Queries;
using TTERP.Application.Models.DTOs.Permissions;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Permissions.Handlers
{
    public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, Response<IReadOnlyList<GetPermissionsDTO>>>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionsQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<Response<IReadOnlyList<GetPermissionsDTO>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _permissionRepository.GetListWithFilterAsync(
                select: permission => permission.Adapt<GetPermissionsDTO>(),
                where: p => p.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || p.IsActive == request.IsActive.Value)
            );

            var result = permissions.OrderBy(x => x.Module)
                                    .ThenBy(x => x.DisplayOrder)
                                    .ToList();

            return Response<IReadOnlyList<GetPermissionsDTO>>.Success(result);
        }
    }
}

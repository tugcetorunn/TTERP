using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Roles.Queries;
using TTERP.Application.Models.DTOs.Roles;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Roles.Handlers
{
    public class GetRoleDetailQueryHandler : IRequestHandler<GetRoleDetailQuery, Response<GetRoleDetailDTO>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;

        public GetRoleDetailQueryHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<Response<GetRoleDetailDTO>> Handle(GetRoleDetailQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetDetailAsync(request.Id, cancellationToken);

            if (role is null)
                return Response<GetRoleDetailDTO>.Fail(404, "Rol bulunamadı.");

            var permissions = await _permissionRepository.GetListWithFilterAsync(
                    select: permission => new RolePermissionDTO
                    {
                        PermissionId = permission.Id,
                        Code = permission.Code,
                        Name = permission.Name,
                        Module = permission.Module,
                        Description = permission.Description,
                        DisplayOrder = permission.DisplayOrder,
                        IsAssigned =
                            role.RolePermissions!
                                .Any(x =>
                                    x.PermissionId ==
                                    permission.Id)
                    },
                    where: permission => permission.IsActive && !permission.IsDeleted);

            var dto = new GetRoleDetailDTO
                {
                    Id = role.Id,
                    Name = role.Name!,
                    NameForUI = role.NameForUI,
                    IsActive = role.IsActive,
                    IsDeleted = role.IsDeleted,
                    UserCount = await _roleRepository.GetUserCountAsync(role.Id),
                    Permissions = permissions
                            .OrderBy(x => x.Module)
                            .ThenBy(x => x.DisplayOrder)
                            .ToList()
                };

            return Response<GetRoleDetailDTO>.Success(dto);
        }
    }
}

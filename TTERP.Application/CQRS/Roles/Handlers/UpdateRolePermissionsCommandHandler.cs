using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Roles.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Roles.Handlers
{
    public class UpdateRolePermissionsCommandHandler : IRequestHandler<UpdateRolePermissionsCommand, Response<bool>>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRolePermissionsCommandHandler(IRolePermissionRepository rolePermissionRepository, RoleManager<Role> roleManager, IUnitOfWork unitOfWork)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

            if (role is null || role.IsDeleted)
            {
                return Response<bool>.Fail(
                    404,
                    "Rol bulunamadı.");
            }

            var existingPermissions = await _rolePermissionRepository.GetByRoleIdAsync(request.RoleId, cancellationToken);

            var requestedIds = request.PermissionIds.Distinct().ToHashSet();

            var permissionsToDelete = existingPermissions.Where(x => !requestedIds.Contains(x.PermissionId)).ToList();

            var existingIds = existingPermissions.Select(x => x.PermissionId).ToHashSet();

            var permissionsToAdd = requestedIds.Where(id => !existingIds.Contains(id))
                                               .Select(id =>
                                                   new RolePermission
                                                   {
                                                       RoleId = request.RoleId,
                                                       PermissionId = id
                                                   })
                                               .ToList();

            if (permissionsToDelete.Count > 0)
            {
                _rolePermissionRepository.RemoveRange(permissionsToDelete);
            }

            if (permissionsToAdd.Count > 0)
            {
                await _rolePermissionRepository.AddRangeAsync(permissionsToAdd, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<bool>.Success(true);
        }
    }
}

using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Roles.Commands;
using TTERP.Domain;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Roles.Handlers
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Response<int>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoleCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork, RoleManager<Role> roleManager, IRolePermissionRepository rolePermissionRepository)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<Response<int>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var roleExists = await _roleManager.RoleExistsAsync(request.Name.Trim());

            if (roleExists)
            {
                return Response<int>.Fail(
                    400,
                    "Bu ada sahip bir rol zaten bulunmaktadır.");
            }

            var role = new Role
            {
                Name = request.Name.Trim(),
                NameForUI = request.NameForUI.Trim()
            };

            var createResult =
                await _roleManager.CreateAsync(role);

            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(x => x.Description).ToArray();

                return Response<int>.Fail(
                    400,
                    errors: errors);
            }

            var permissionIds = request.PermissionIds
                .Distinct()
                .ToList();

            foreach (var permissionId in permissionIds)
            {
                await _rolePermissionRepository.AddAsync(
                    new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = permissionId
                    });
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(201, role.Id, "Rol başarıyla kaydedilmiştir.");
        }
    }
}

using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Models;
using TTERP.Persistence.Contexts;
using TTERP.Persistence.Repositories.Abstract;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<Role?> GetDetailAsync(int roleId, CancellationToken cancellationToken = default)
        {
            return await context.Roles
                                .Include(x => x.RolePermissions)!
                                    .ThenInclude(x => x.Permission)
                                .FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);
        }

        public async Task<IReadOnlyList<GetRolesDTO>> GetRolesWithCountsAsync(bool? isActive, bool? isDeleted, CancellationToken cancellationToken = default)
        {
            var roles = await(from role in context.Roles
                            where role.IsDeleted == (isDeleted ?? false) && (!isActive.HasValue || role.IsActive == isActive.Value)
                            select new GetRolesDTO
                            {
                                Id = role.Id,

                                Name = role.Name!,

                                NameForUI = role.NameForUI,

                                UserCount = context.UserRoles.Count(
                                    userRole => userRole.RoleId == role.Id),

                                PermissionCount = context.RolePermissions.Count(
                                    rolePermission =>
                                        rolePermission.RoleId == role.Id),

                                IsActive = role.IsActive,

                                IsDeleted = role.IsDeleted
                            }).AsNoTracking()
                            .OrderBy(x => x.NameForUI)
                            .ToListAsync(cancellationToken);

            return roles;
        }

        public async Task<int> GetUserCountAsync(int roleId)
        {
            return await context.UserRoles.CountAsync(x => x.RoleId == roleId);
        }
    }
}

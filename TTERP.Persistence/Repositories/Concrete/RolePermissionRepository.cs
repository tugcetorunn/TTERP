using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Persistence.Contexts;
using TTERP.Persistence.Repositories.Abstract;
using Task = System.Threading.Tasks.Task;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class RolePermissionRepository : BaseRepository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task AddRangeAsync(List<RolePermission> permissionsToAdd, CancellationToken cancellationToken = default)
        {
            await context.RolePermissions.AddRangeAsync(permissionsToAdd, cancellationToken);
        }

        public async Task<List<RolePermission>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default)
        {
            return await context.RolePermissions
                                .Where(x => x.RoleId == roleId)
                                .ToListAsync(cancellationToken);
        }

        public void RemoveRange(List<RolePermission> permissionsToDelete)
        {
            context.RolePermissions.RemoveRange(permissionsToDelete);
        }
    }
}

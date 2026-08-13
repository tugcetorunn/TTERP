using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task = System.Threading.Tasks.Task;
using TTERP.Domain.Entities;

namespace TTERP.Domain.Interfaces.RepositoryInterfaces
{
    public interface IRolePermissionRepository : IBaseRepository<RolePermission>
    {
        Task AddRangeAsync(List<RolePermission> permissionsToAdd, CancellationToken cancellationToken = default);
        Task<List<RolePermission>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);
        void RemoveRange(List<RolePermission> permissionsToDelete);
    }
}

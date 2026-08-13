using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Models;

namespace TTERP.Domain.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role?> GetDetailAsync(int roleId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<GetRolesDTO>> GetRolesWithCountsAsync(bool? isActive, bool? isDeleted, CancellationToken cancellationToken = default);
        Task<int> GetUserCountAsync(int roleId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Models;

namespace TTERP.Domain.Interfaces
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<bool> IsManagerAsync(int employeeId);
        Task<List<int>> GetTeamEmployeeIdsAsync(int managerId);
        Task<int> GetMaxRegistrationNumberAsync(CancellationToken cancellationToken);
        Task<string?> GetMaxInternalPhoneAsync(CancellationToken cancellationToken);
        Task<bool> IsEmployeeInAnyTeamAsync(int memberId, CancellationToken cancellationToken);
        Task<IReadOnlyList<GetEmployeesDTO>> GetEmployeesWithRoleAsync(bool? isActive, bool? isDeleted, CancellationToken cancellationToken = default);
    }
}

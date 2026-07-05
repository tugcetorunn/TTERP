using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Persistence.Contexts;
using TTERP.Persistence.Repositories.Abstract;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<bool> IsManagerAsync(int employeeId)
        {
            return await context.Set<TeamManager>()
                .AnyAsync(tm => tm.ManagerId == employeeId && !tm.IsDeleted && tm.IsActive); // TODO: isdeleted ve isactive i kaldırıp hepsini getirecek mi gör
        }

        public async Task<List<int>> GetTeamEmployeeIdsAsync(int managerId)
        {
            var managedTeamIds = await context.Set<TeamManager>()
                .Where(tm => tm.ManagerId == managerId && !tm.IsDeleted && tm.IsActive)
                .Select(tm => tm.TeamId)
                .ToListAsync();

            if (managedTeamIds.Any())
                return new List<int>();

            return await context.Set<Employee>()
                .Where(e => e.TeamId.HasValue &&
                            managedTeamIds.Contains(e.TeamId.Value) &&
                            !e.IsDeleted && e.IsActive)
                .Select(e => e.Id)
                .ToListAsync();

        }

        public async Task<int> GetMaxRegistrationNumberAsync(CancellationToken cancellationToken)
        {
            return await context.Set<Employee>()
                                .MaxAsync(e => (int?)e.RegistrationNumber, cancellationToken) ?? 0;
        }

        public async Task<string?> GetMaxInternalPhoneAsync(CancellationToken cancellationToken)
        {
            return await context.Set<Employee>()
                                .MaxAsync(e => e.InternalPhone, cancellationToken);
        }

        public async Task<bool> IsEmployeeInAnyTeamAsync(int memberId, CancellationToken cancellationToken)
        {
            return await context.Set<Employee>()
                          .AnyAsync(e => e.Id == memberId && e.TeamId.HasValue && !e.IsDeleted && e.IsActive, cancellationToken);
        }
    }
}

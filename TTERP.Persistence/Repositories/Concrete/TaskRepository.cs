using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Interfaces;
using TTERP.Persistence.Repositories.Abstract;
using TTERP.Persistence.Contexts;
using TTERP.Domain.Entities;
using Task = TTERP.Domain.Entities.Task;
using Microsoft.EntityFrameworkCore;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class TaskRepository : BaseRepository<Task>, ITaskRepository
    {
        public TaskRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<List<Task>> GetTasksByAssignmentRoleAsync(int? roleCode, List<int> targetEmployeeIds, CancellationToken cancellationToken)
        {
            return await context.Tasks
                                .Include(t => t.TaskAssignments!)
                                    .ThenInclude(ta => ta.Role == roleCode && targetEmployeeIds.Contains(ta.EmployeeId))
                                .Where(t => t.IsActive && !t.IsDeleted)
                                .ToListAsync(cancellationToken);
        }
    }
}

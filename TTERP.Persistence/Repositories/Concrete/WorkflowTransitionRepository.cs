using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Persistence.Contexts;
using TTERP.Persistence.Repositories.Abstract;

namespace TTERP.Persistence.Repositories.Concrete
{
    public class WorkflowTransitionRepository : BaseRepository<WorkflowTransition>, IWorkflowTransitionRepository
    {
        public WorkflowTransitionRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<List<WorkflowTransition>> GetAllowedTransitionsAsync(
        int workflowType,
        int fromStatusCode,
        CancellationToken cancellationToken = default)
        {
            return await context.WorkflowTransitions
                .AsNoTracking()
                .Where(x =>
                    x.WorkflowType == workflowType &&
                    x.FromStatusCode == fromStatusCode &&
                    x.IsActive &&
                    !x.IsDeleted)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken);
        }

        public async Task<WorkflowTransition?> GetTransitionAsync(
            int workflowType,
            int fromStatusCode,
            int toStatusCode,
            CancellationToken cancellationToken = default)
        {
            return await context.WorkflowTransitions
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.WorkflowType == workflowType &&
                    x.FromStatusCode == fromStatusCode &&
                    x.ToStatusCode == toStatusCode &&
                    x.IsActive &&
                    !x.IsDeleted,
                    cancellationToken);
        }
    }
}

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
    public class WorkflowHistoryRepository : BaseRepository<WorkflowHistory>, IWorkflowHistoryRepository
    {
        public WorkflowHistoryRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<List<WorkflowHistory>> GetByRecordAsync(int workflowType, int recordId, CancellationToken cancellationToken = default)
        {
            return await context.WorkflowHistories
                                .AsNoTracking()
                                .Include(x => x.Employee)
                                .Where(x =>
                                    x.WorkflowType == workflowType &&
                                    x.RecordId == recordId &&
                                    x.IsActive &&
                                    !x.IsDeleted)
                                .OrderByDescending(x => x.ChangeDate)
                                .ToListAsync(cancellationToken);
        }

        public async Task<List<WorkflowHistory>> GetByWorkflowTypeAsync(int workflowType, CancellationToken cancellationToken)
        {
            return await context.WorkflowHistories
                                .AsNoTracking()
                                .Include(x => x.Employee)
                                .Where(x =>
                                    x.WorkflowType == workflowType &&
                                    x.IsActive &&
                                    !x.IsDeleted)
                                .OrderByDescending(x => x.ChangeDate)
                                .ToListAsync(cancellationToken);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Domain.Interfaces.RepositoryInterfaces
{
    public interface IWorkflowHistoryRepository : IBaseRepository<WorkflowHistory>
    {
        Task<List<WorkflowHistory>> GetByRecordAsync(int workflowType, int recordId, CancellationToken cancellationToken = default);
        Task<List<WorkflowHistory>> GetByWorkflowTypeAsync(int workflowType, CancellationToken cancellationToken);
    }
}

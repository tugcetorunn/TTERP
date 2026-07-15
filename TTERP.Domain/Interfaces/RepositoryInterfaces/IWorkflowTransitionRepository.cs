using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Domain.Interfaces.RepositoryInterfaces
{
    public interface IWorkflowTransitionRepository : IBaseRepository<WorkflowTransition>
    {
        Task<List<WorkflowTransition>> GetAllowedTransitionsAsync(
        int workflowType,
        int fromStatusCode,
        CancellationToken cancellationToken = default);

        Task<WorkflowTransition?> GetTransitionAsync(
            int workflowType,
            int fromStatusCode,
            int toStatusCode,
            CancellationToken cancellationToken = default);
    }
}

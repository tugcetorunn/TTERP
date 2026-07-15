using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Supplies;
using TTERP.Domain.Entities;

namespace TTERP.Application.Interfaces
{
    public interface IWorkflowService
    {
        Task<List<AllowedWorkflowTransitionDTO>> GetAllowedTransitionsAsync(
        int workflowType,
        int currentStatusCode,
        CancellationToken cancellationToken = default);

        Task<WorkflowTransition?> ValidateTransitionAsync(
            int workflowType,
            int fromStatusCode,
            int toStatusCode,
            CancellationToken cancellationToken = default);
    }
}

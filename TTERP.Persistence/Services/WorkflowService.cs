using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Interfaces;
using TTERP.Application.Models.DTOs.Supplies;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.RepositoryInterfaces;

namespace TTERP.Persistence.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IWorkflowTransitionRepository _workflowTransitionRepository;
        private readonly IParameterValueRepository _parameterValueRepository;

        public WorkflowService(
            IWorkflowTransitionRepository workflowTransitionRepository,
            IParameterValueRepository parameterValueRepository)
        {
            _workflowTransitionRepository = workflowTransitionRepository;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<List<AllowedWorkflowTransitionDTO>> GetAllowedTransitionsAsync(
    int workflowType,
    int currentStatusCode,
    CancellationToken cancellationToken = default)
        {
            var transitions = await _workflowTransitionRepository.GetAllowedTransitionsAsync(
                workflowType,
                currentStatusCode,
                cancellationToken);

            if (!transitions.Any())
            {
                return new List<AllowedWorkflowTransitionDTO>();
            }

            var parameterType = GetStatusParameterType(workflowType);

            var parameterValues = await _parameterValueRepository
                .GetParamValuesByParamTypeAsync(
                    parameterType,
                    1,
                    cancellationToken);

            var parameterDictionary = parameterValues
                .Where(x => x != null)
                .ToDictionary(x => x!.ParamCode);

            return transitions.Select(transition =>
            {
                parameterDictionary.TryGetValue(
                    transition.ToStatusCode,
                    out var targetStatus);

                return new AllowedWorkflowTransitionDTO
                {
                    TargetStatusCode = transition.ToStatusCode,
                    StatusName = targetStatus?.ParamValue ?? "Tanımsız",
                    StatusShortCode = targetStatus?.ShortCode,
                    ActionName = targetStatus?.ParamValue ?? "İlerle",
                    BadgeColor = targetStatus?.BadgeColor,
                    Icon = targetStatus?.Icon,
                    RequiresConfirmation = transition.RequiresConfirmation
                };
            }).ToList();
        }

        public Task<WorkflowTransition?> ValidateTransitionAsync(
            int workflowType,
            int fromStatusCode,
            int toStatusCode,
            CancellationToken cancellationToken = default)
        {
            return _workflowTransitionRepository.GetTransitionAsync(
                workflowType,
                fromStatusCode,
                toStatusCode,
                cancellationToken);
        }

        private static string GetStatusParameterType(int workflowType)
        {
            return workflowType switch
            {
                1 => "SupplyStatus",
                2 => "ProductionStatus",
                3 => "OrderStatus",
                7 => "ShippingStatus",
                _ => throw new ArgumentOutOfRangeException(
                    nameof(workflowType),
                    "Tanımsız workflow tipi.")
            };
        }
    }
}

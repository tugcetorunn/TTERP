using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Productions.Queries;
using TTERP.Application.Interfaces;
using TTERP.Application.Models.DTOs.Productions;
using TTERP.Application.Models.DTOs.Supplies;
using TTERP.Application.Models.DTOs.WorkflowHistories;
using TTERP.Application.Workflows;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Productions.Handlers
{
    public class GetProductionsQueryHandler : IRequestHandler<GetProductionsQuery, Response<IReadOnlyList<GetProductionsDTO>>>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IWorkflowHistoryRepository _workflowHistoryRepository;
        private readonly IWorkflowService _workflowService;

        public GetProductionsQueryHandler(IProductionRepository productionRepository, IParameterValueRepository parameterValueRepository, IWorkflowService workflowService, IWorkflowHistoryRepository workflowHistoryRepository)
        {
            _productionRepository = productionRepository;
            _parameterValueRepository = parameterValueRepository;
            _workflowService = workflowService;
            _workflowHistoryRepository = workflowHistoryRepository;
        }

        public async Task<Response<IReadOnlyList<GetProductionsDTO>>> Handle(GetProductionsQuery request, CancellationToken cancellationToken)
        {
            var productions = await _productionRepository.GetListWithFilterAsync(
            select: production => production,
            where: production =>
                production.IsDeleted == (request.IsDeleted ?? false) &&
                (!request.IsActive.HasValue ||
                 production.IsActive == request.IsActive.Value),
            include: query => query
                .Include(production => production.Product)
                .Include(production => production.TargetWarehouse)
                .Include(production => production.ProductionItems)!
                    .ThenInclude(item => item.Material)
                .Include(production => production.ProductionItems)!
                    .ThenInclude(item => item.SourceWarehouse)!
                .Include(production => production.ProductionItems)!
                    .ThenInclude(item => item.MaterialStockReservations)!
                .Include(x => x.ProductionProgresses)!
                    .ThenInclude(x => x.Employee)!);

            var statusValues = await _parameterValueRepository
                .GetParamValuesByParamTypeAsync(
                    "ProductionStatus",
                    1,
                    cancellationToken);

            var statusDictionary = statusValues
                .Where(value => value != null)
                .ToDictionary(value => value!.ParamCode);

            var workflowHistories = await _workflowHistoryRepository.GetByWorkflowTypeAsync(2, cancellationToken);

            var historyLookup = workflowHistories
                                    .GroupBy(x => x.RecordId)
                                    .ToDictionary(
                                        x => x.Key,
                                        x => x.ToList());

            var transitionsByStatus =
                new Dictionary<int, List<AllowedWorkflowTransitionDTO>>();

            var result = new List<GetProductionsDTO>();

            foreach (var production in productions)
            {
                var dto = production.Adapt<GetProductionsDTO>();

                if (production.ProductionStatus.HasValue &&
                    statusDictionary.TryGetValue(
                        production.ProductionStatus.Value,
                        out var status))
                {
                    dto.ProductionStatusName = status.ParamValue;
                    dto.ProductionStatusShortCode = status.ShortCode;
                    dto.ProductionStatusBadgeColor = status.BadgeColor;
                    dto.ProductionStatusIcon = status.Icon;

                    if (!transitionsByStatus.TryGetValue(
                            production.ProductionStatus.Value,
                            out var transitions))
                    {
                        transitions = await _workflowService.GetAllowedTransitionsAsync(
                            workflowType: 2,
                            currentStatusCode: production.ProductionStatus.Value,
                            cancellationToken: cancellationToken);

                        transitionsByStatus[production.ProductionStatus.Value] =
                            transitions;
                    }

                    dto.AllowedTransitions = transitions;

                    dto.Actions = WorkflowActionHelper.CreateActions(
                        workflowType: 2,
                        statusShortCode: status.ShortCode);
                }

                if (historyLookup.TryGetValue(production.Id, out var histories))
                {
                    dto.WorkflowHistories =
                        histories.Select(x =>
                        {
                            statusDictionary.TryGetValue(
                                x.FromStatusCode ?? 0,
                                out var fromStatus);

                            statusDictionary.TryGetValue(
                                x.ToStatusCode,
                                out var toStatus);

                            return new GetWorkflowHistoryDTO
                            {
                                Id = x.Id,
                                WorkflowType = x.WorkflowType,
                                RecordId = x.RecordId,

                                FromStatusCode = x.FromStatusCode,
                                FromStatusName = fromStatus?.ParamValue,

                                ToStatusCode = x.ToStatusCode,
                                ToStatusName = toStatus?.ParamValue,

                                EmployeeId = x.EmployeeId,
                                EmployeeName =
                                    x.Employee == null
                                    ? null
                                    : x.Employee.FullName,

                                Note = x.Note,
                                ChangeDate = x.ChangeDate
                            };
                        }).ToList();
                }

                result.Add(dto);
            }

            return Response<IReadOnlyList<GetProductionsDTO>>.Success(result);
        }
    }
}

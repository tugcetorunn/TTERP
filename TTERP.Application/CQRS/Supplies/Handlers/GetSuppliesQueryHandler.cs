using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ProductWarehouses.Queries;
using TTERP.Application.CQRS.Supplies.Queries;
using TTERP.Application.Interfaces;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.Supplies;
using TTERP.Application.Workflows;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Supplies.Handlers
{
    public class GetSuppliesQueryHandler : IRequestHandler<GetSuppliesQuery, Response<IReadOnlyList<GetSuppliesDTO>>>
    {
        private readonly ISupplyRepository _supplyRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IWorkflowService _workflowService;

        public GetSuppliesQueryHandler(ISupplyRepository supplyRepository, IWorkflowService workflowService, IParameterValueRepository parameterValueRepository)
        {
            _supplyRepository = supplyRepository;
            _workflowService = workflowService;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<IReadOnlyList<GetSuppliesDTO>>> Handle(GetSuppliesQuery request, CancellationToken cancellationToken)
        {
            var supplies = await _supplyRepository.GetListWithFilterAsync(
                select: s => s,
                where: s =>
                    s.IsDeleted == (request.IsDeleted ?? false) &&
                    (!request.IsActive.HasValue || s.IsActive == request.IsActive.Value),
                include: x => x
                    .Include(x => x.Supplier)
                    .Include(x => x.Employee)
                    .Include(x => x.SupplyItems)!
                        .ThenInclude(i => i.Material)
                    .Include(x => x.SupplyItems)!
                        .ThenInclude(i => i.Warehouse)!);

            var statusValues = await _parameterValueRepository
                .GetParamValuesByParamTypeAsync(
                    "SupplyStatus",
                    1,
                    cancellationToken);

            var statusDictionary = statusValues
                .Where(x => x != null)
                .ToDictionary(x => x!.ParamCode);

            var result = new List<GetSuppliesDTO>();
            var transitionsByStatus =
                new Dictionary<int, List<AllowedWorkflowTransitionDTO>>();

            foreach (var supply in supplies)
            {
                var dto = supply.Adapt<GetSuppliesDTO>();

                if (supply.SupplyStatus.HasValue &&
                    statusDictionary.TryGetValue(
                        supply.SupplyStatus.Value,
                        out var status))
                {
                    dto.SupplyStatusName = status.ParamValue;
                    dto.SupplyStatusShortCode = status.ShortCode;
                    dto.SupplyStatusBadgeColor = status.BadgeColor;
                    dto.SupplyStatusIcon = status.Icon;

                    if (!transitionsByStatus.TryGetValue(
                            supply.SupplyStatus.Value,
                            out var allowedTransitions))
                    {
                        allowedTransitions = await _workflowService
                            .GetAllowedTransitionsAsync(
                                workflowType: 1,
                                currentStatusCode: supply.SupplyStatus.Value,
                                cancellationToken: cancellationToken);

                        transitionsByStatus[supply.SupplyStatus.Value] =
                            allowedTransitions;
                    }

                    dto.AllowedTransitions = allowedTransitions;
                    dto.Actions = WorkflowActionHelper.CreateActions(
                        workflowType: 1,
                        status.ShortCode);
                }

                result.Add(dto);
            }

            return Response<IReadOnlyList<GetSuppliesDTO>>.Success(result);
        }
    }
}

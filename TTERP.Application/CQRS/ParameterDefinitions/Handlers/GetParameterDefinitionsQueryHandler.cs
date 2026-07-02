using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ParameterDefinitions.Queries;
using TTERP.Application.Models.DTOs.Invoices;
using TTERP.Application.Models.DTOs.ParameterDefinitions;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ParameterDefinitions.Handlers
{
    public class GetParameterDefinitionsQueryHandler : IRequestHandler<GetParameterDefinitionsQuery, Response<IReadOnlyList<GetParameterDefinitionsDTO>>>
    {
        private readonly IParameterDefinitionRepository _parameterDefinitionRepository;

        public GetParameterDefinitionsQueryHandler(IParameterDefinitionRepository parameterDefinitionRepository)
        {
            _parameterDefinitionRepository = parameterDefinitionRepository;
        }

        public async Task<Response<IReadOnlyList<GetParameterDefinitionsDTO>>> Handle(GetParameterDefinitionsQuery request, CancellationToken cancellationToken)
        {
            var definitions = await _parameterDefinitionRepository.GetListWithFilterAsync(
                select: i => i.Adapt<GetParameterDefinitionsDTO>(),
                where: i => i.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || i.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetParameterDefinitionsDTO>>.Success(definitions.ToList());
        }
    }
}

using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ParameterValues.Queries;
using TTERP.Application.Models.DTOs.ParameterValues;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ParameterValues.Handlers
{
    public class GetParameterValuesByTypeQueryHandler : IRequestHandler<GetParameterValuesByTypeQuery, Response<IReadOnlyList<GetParameterValuesByTypeDTO>>>
    {
        private readonly IParameterValueRepository _parameterValueRepository;

        public GetParameterValuesByTypeQueryHandler(IParameterValueRepository parameterValueRepository)
        {
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<IReadOnlyList<GetParameterValuesByTypeDTO>>> Handle(GetParameterValuesByTypeQuery request, CancellationToken cancellationToken)
        {
            var values = await _parameterValueRepository.GetParamValuesByParamTypeAsync(request.ParamType, 1, cancellationToken);

            var result = values
                .Select(x => x.Adapt<GetParameterValuesByTypeDTO>())
                .ToList();

            return Response<IReadOnlyList<GetParameterValuesByTypeDTO>>.Success(result);
        }
    }
}

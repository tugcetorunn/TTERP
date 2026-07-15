using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ParameterValues.Queries;
using TTERP.Application.Models.DTOs.Invoices;
using TTERP.Application.Models.DTOs.ParameterValues;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ParameterValues.Handlers
{
    public class GetParameterValuesQueryHandler : IRequestHandler<GetParameterValuesQuery, Response<IReadOnlyList<GetParameterValuesDTO>>>
    {
        private readonly IParameterValueRepository _parameterValueRepository;

        public GetParameterValuesQueryHandler(IParameterValueRepository parameterValueRepository)
        {
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<IReadOnlyList<GetParameterValuesDTO>>> Handle(GetParameterValuesQuery request, CancellationToken cancellationToken)
        {
            var values = await _parameterValueRepository.GetListWithFilterAsync(
                select: v => v.Adapt<GetParameterValuesDTO>(),
                where: v => v.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || v.IsActive == request.IsActive.Value) && (!request.LanguageId.HasValue ||
                v.LanguageId == request.LanguageId.Value),
                include: v => v.Include(v => v.ParameterDefinition)!);

            return Response<IReadOnlyList<GetParameterValuesDTO>>.Success(values.ToList());
        }
    }
}

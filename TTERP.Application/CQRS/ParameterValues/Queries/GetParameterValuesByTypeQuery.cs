using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ParameterValues;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ParameterValues.Queries
{
    public class GetParameterValuesByTypeQuery : IRequest<Response<IReadOnlyList<GetParameterValuesByTypeDTO>>>
    {
        public string ParamType { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public GetParameterValuesByTypeQuery(string paramType, bool? isActive, bool? isDeleted)
        {
            ParamType = paramType;
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

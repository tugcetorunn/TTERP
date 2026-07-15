using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ParameterDefinitions;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ParameterDefinitions.Queries
{
    public class GetParameterDefinitionsQuery : IRequest<Response<IReadOnlyList<GetParameterDefinitionsDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetParameterDefinitionsQuery(bool? isActive, bool? isDeleted)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

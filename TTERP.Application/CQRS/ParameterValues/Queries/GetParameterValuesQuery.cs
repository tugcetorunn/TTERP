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
    public class GetParameterValuesQuery : IRequest<Response<IReadOnlyList<GetParameterValuesDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? LanguageId { get; set; }
        public GetParameterValuesQuery(bool? isActive, bool? isDeleted, int? languageId)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
            LanguageId = languageId;
        }
    }
}

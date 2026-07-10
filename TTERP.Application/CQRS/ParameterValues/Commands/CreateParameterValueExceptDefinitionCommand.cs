using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ParameterValues.Commands
{
    public class CreateParameterValueExceptDefinitionCommand : IRequest<Response<int>>
    {
        public string ParamType { get; set; }
        public int ParamCode { get; set; }
        public string ParamValue { get; set; }
        public string? Description { get; set; }
        public int LanguageId { get; set; }
    }
}

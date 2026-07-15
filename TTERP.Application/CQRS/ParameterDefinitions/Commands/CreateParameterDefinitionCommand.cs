using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ParameterValues.Commands;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ParameterDefinitions.Commands
{
    public class CreateParameterDefinitionCommand : IRequest<Response<int>>
    {
        public string ParamType { get; set; }
        public string Description { get; set; }
        public string DataType { get; set; }
        public int? DefaultValue { get; set; }
        public List<CreateParameterValueCommand>? ParameterValues { get; set; }
    }
}

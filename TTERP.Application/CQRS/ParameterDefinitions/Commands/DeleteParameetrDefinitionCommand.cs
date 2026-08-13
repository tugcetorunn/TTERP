using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ParameterDefinitions.Commands
{
    public class DeleteParameetrDefinitionCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteParameetrDefinitionCommand(int id)
        {
            Id = id;
        }
    }
}

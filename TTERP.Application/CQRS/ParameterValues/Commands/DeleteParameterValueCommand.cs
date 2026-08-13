using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.ParameterValues.Commands
{
    public class DeleteParameterValueCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public DeleteParameterValueCommand(int id)
        {
            Id = id;
        }
    }
}

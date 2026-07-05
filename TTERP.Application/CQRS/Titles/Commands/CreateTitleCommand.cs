using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Titles.Commands
{
    public class CreateTitleCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}

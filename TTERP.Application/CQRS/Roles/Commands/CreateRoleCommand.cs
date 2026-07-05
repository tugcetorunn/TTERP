using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Roles.Commands
{
    public class CreateRoleCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string NameForUI { get; set; }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Roles.Commands
{
    public class UpdateRolePermissionsCommand : IRequest<Response<bool>>
    {
        public int RoleId { get; set; }

        public ICollection<int> PermissionIds { get; set; } = new List<int>();
    }
}

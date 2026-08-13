using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Permissions;
using TTERP.Application.Models.DTOs.Roles;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Permissions.Queries
{
    public class GetPermissionsQuery : IRequest<Response<IReadOnlyList<GetPermissionsDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetPermissionsQuery(bool? isActive, bool? isDeleted)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.Roles;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Roles.Queries
{
    public class GetRolesQuery : IRequest<Response<IReadOnlyList<GetRolesDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetRolesQuery(bool? isActive, bool? isDeleted)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

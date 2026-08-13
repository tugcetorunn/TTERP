using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Roles;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Roles.Queries
{
    public class GetRoleDetailQuery : IRequest<Response<GetRoleDetailDTO>>
    {
        public int Id { get; set; }
        public GetRoleDetailQuery(int id)
        {
            Id = id;
        }
    }
}

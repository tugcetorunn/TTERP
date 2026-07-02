using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Materials;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Materials.Queries
{
    public class GetMaterialsQuery : IRequest<Response<IReadOnlyList<GetMaterialsDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

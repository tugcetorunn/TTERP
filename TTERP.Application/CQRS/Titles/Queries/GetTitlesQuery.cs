using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ProductWarehouses;
using TTERP.Application.Models.DTOs.Titles;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Titles.Queries
{
    public class GetTitlesQuery : IRequest<Response<IReadOnlyList<GetTitlesDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

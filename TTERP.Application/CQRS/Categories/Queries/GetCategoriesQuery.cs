using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Categories;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Categories.Queries
{
    public class GetCategoriesQuery : IRequest<Response<IReadOnlyList<GetCategoriesDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetCategoriesQuery(bool? isActive, bool? isDeleted)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

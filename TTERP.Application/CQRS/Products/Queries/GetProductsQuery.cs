using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Products;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Products.Queries
{
    public class GetProductsQuery : IRequest<Response<IReadOnlyList<GetProductsDTO>>>
    {
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public GetProductsQuery(bool? isActive, bool? isDeleted)
        {
            IsActive = isActive;
            IsDeleted = isDeleted;
        }
    }
}

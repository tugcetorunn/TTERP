using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Customers.Queries;
using TTERP.Application.Models.DTOs.Customers;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Customers.Handlers
{
    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, Response<IReadOnlyList<GetCustomersDTO>>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomersQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Response<IReadOnlyList<GetCustomersDTO>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetListWithFilterAsync(
                select: c => c.Adapt<GetCustomersDTO>(),
                where: c => c.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || c.IsActive == request.IsActive.Value),
                include: s => s.Include(s => s.Country).Include(s => s.City).Include(s => s.Town).Include(s => s.District).Include(s => s.Neighborhood)!
                );

            return Response<IReadOnlyList<GetCustomersDTO>>.Success(customers.ToList());
        }
    }
}

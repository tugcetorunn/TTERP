using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Payments.Queries;
using TTERP.Application.Models.DTOs.Invoices;
using TTERP.Application.Models.DTOs.Payments;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Payments.Handlers
{
    public class GetPaymentsQueryHandler : IRequestHandler<GetPaymentsQuery, Response<IReadOnlyList<GetPaymentsDTO>>>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetPaymentsQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Response<IReadOnlyList<GetPaymentsDTO>>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
        {
            var payments = await _paymentRepository.GetListWithFilterAsync(
                select: p => p.Adapt<GetPaymentsDTO>(),
                where: p => p.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || p.IsActive == request.IsActive.Value));

            return Response<IReadOnlyList<GetPaymentsDTO>>.Success(payments.ToList());
        }
    }
}

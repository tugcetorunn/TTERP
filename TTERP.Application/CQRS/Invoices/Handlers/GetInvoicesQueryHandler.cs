using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Invoices.Queries;
using TTERP.Application.Models.DTOs.Invoices;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Invoices.Handlers
{
    public class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, Response<IReadOnlyList<GetInvoicesDTO>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public GetInvoicesQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<Response<IReadOnlyList<GetInvoicesDTO>>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _invoiceRepository.GetListWithFilterAsync(
                select: i => i.Adapt<GetInvoicesDTO>(),
                where: i => i.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || i.IsActive == request.IsActive.Value) &&
                (!request.OrderId.HasValue || i.OrderId == request.OrderId.Value));

            return Response<IReadOnlyList<GetInvoicesDTO>>.Success(invoices.ToList());
        }
    }
}

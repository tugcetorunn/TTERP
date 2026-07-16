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
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Payments.Handlers
{
    public class GetPaymentsQueryHandler : IRequestHandler<GetPaymentsQuery, Response<IReadOnlyList<GetPaymentsDTO>>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IParameterValueRepository _parameterValueRepository;

        public GetPaymentsQueryHandler(IPaymentRepository paymentRepository, IParameterValueRepository parameterValueRepository)
        {
            _paymentRepository = paymentRepository;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<IReadOnlyList<GetPaymentsDTO>>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
        {
            var payments = await _paymentRepository.GetListWithFilterAsync(
                select: p => p.Adapt<GetPaymentsDTO>(),
                where: p => p.IsDeleted == (request.IsDeleted ?? false) && (!request.IsActive.HasValue || p.IsActive == request.IsActive.Value) &&
                (!request.OrderId.HasValue || p.OrderId == request.OrderId.Value));

            var statusValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                "PaymentStatus",
                                1,
                                cancellationToken);

            var statusDictionary = statusValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);

            var typeValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                "PaymentType",
                                1,
                                cancellationToken);

            var typeDictionary = typeValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);

            var currencyValues = await _parameterValueRepository.GetParamValuesByParamTypeAsync(
                                "Currency",
                                1,
                                cancellationToken);

            var currencyDictionary = currencyValues.Where(value => value != null)
                                           .GroupBy(value => value!.ParamCode)
                                           .ToDictionary(
                                               group => group.Key,
                                               group => group.First()!.ParamValue);

            foreach (var payment in payments)
            {
                payment.PaymentStatusName = statusDictionary.GetValueOrDefault(payment.PaymentStatus);
                payment.PaymentTypeName = typeDictionary.GetValueOrDefault(payment.PaymentType);
                payment.CurrencyName = currencyDictionary.GetValueOrDefault(payment.Currency)!;
            }

            return Response<IReadOnlyList<GetPaymentsDTO>>.Success(payments.ToList());
        }
    }
}

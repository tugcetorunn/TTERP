using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Payments.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Payments.Handlers
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Response<int>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentCommandHandler(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IParameterValueRepository parameterValueRepository, IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _parameterValueRepository = parameterValueRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<int>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.FindAsync(request.OrderId);
            if (order == null)
            {
                return Response<int>.Fail(404, "Ödeme yapılmak istenen sipariş bulunamadı.");
            }

            if (request.Currency != order.Currency)
            {
                return Response<int>.Fail(
                    400,
                    "Ödeme para birimi sipariş para birimiyle aynı olmalıdır.");
            }

            var payment = request.Adapt<Payment>();

            decimal totalPaidAmountBeforeThis = await _paymentRepository.GetTotalPaidAmountByOrderIdAsync(request.OrderId, cancellationToken);

            decimal totalPaidAmount = request.Amount + totalPaidAmountBeforeThis;

            if (totalPaidAmount > order.FinalAmount)
            {
                return Response<int>.Fail(400, "Ödeme miktarı siparişin toplam tutarını aşamaz.");
            }
            else if (totalPaidAmount == order.FinalAmount)
            {
                order.PaymentStatus = await _parameterValueRepository.ParamValueToParamCode("PaymentStatus", "Paid", cancellationToken); // Ödeme tamamlandı
            }
            else
            {
                order.PaymentStatus = await _parameterValueRepository.ParamValueToParamCode("PaymentStatus", "PartiallyPaid", cancellationToken); // Kısmi ödeme yapıldı
            }

            await _paymentRepository.AddAsync(payment);

            _orderRepository.Update(order);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Response<int>.Success(payment.Id, 201, "Ödeme başarıyla kaydedildi ve sipariş ödeme durumu güncellendi.");
        }
    }
}

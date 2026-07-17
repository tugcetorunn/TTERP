using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Invoices.Commands;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.ServiceInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Invoices.Handlers
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Response<int>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IParameterValueRepository _parameterValueRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrencyService _currencyService;

        public CreateInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IOrderRepository orderRepository, IUnitOfWork unitOfWork, ICurrencyService currencyService, IParameterValueRepository parameterValueRepository)
        {
            _invoiceRepository = invoiceRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _currencyService = currencyService;
            _parameterValueRepository = parameterValueRepository;
        }

        public async Task<Response<int>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderWithOrderItemsAsync(request.OrderId, cancellationToken);

            if (order == null)
                return Response<int>.Fail(404, "Faturalandırılmak istenen sipariş bulunamadı.");

            if (order.OrderItems == null)
                return Response<int>.Fail(404, "Siparişin faturalandırılabilir kalemi bulunamadı.");

            var approvedStatus = await _parameterValueRepository.GetByShortCodeAsync(
                                            "OrderStatus",
                                            "approved",
                                            1,
                                            cancellationToken);

            var completedStatus = await _parameterValueRepository.GetByShortCodeAsync(
                                            "OrderStatus",
                                            "completed",
                                            1,
                                            cancellationToken);

            if (approvedStatus == null || completedStatus == null)
            {
                return Response<int>.Fail(
                    500,
                    "Sipariş durumları tanımlanmamış.");
            }

            var canCreateInvoice = order.OrderStatus == approvedStatus.ParamCode || order.OrderStatus == completedStatus.ParamCode; // sipariş onaylanmış veya tamamlanmışsa fatura oluşturabiliriz

            if (!canCreateInvoice)
            {
                return Response<int>.Fail(
                    400,
                    "Fatura yalnızca onaylanmış veya tamamlanmış siparişler için oluşturulabilir.");
            }

            var completedPaymentStatus = await _parameterValueRepository.GetByShortCodeAsync(
                                                    "PaymentStatus",
                                                    "paid",
                                                    1,
                                                    cancellationToken);

            var hasActiveInvoice = order.Invoices?.Any(invoice => invoice.IsActive && !invoice.IsDeleted) == true;

            if (hasActiveInvoice)
            {
                return Response<int>.Fail(
                    400,
                    "Bu sipariş için daha önce fatura oluşturulmuş.");
            }

            if (completedPaymentStatus == null)
            {
                return Response<int>.Fail(
                    500,
                    "Tamamlanmış ödeme durum parametresi tanımlanmamış.");
            }

            var paidAmountInOrderCurrency = order.Payments?.Where(payment => payment.IsActive &&
                                                            !payment.IsDeleted &&
                                                            payment.PaymentStatus ==
                                                                completedPaymentStatus.ParamCode)
                                                        .Sum(payment => payment.Amount) ?? 0m;

            if (paidAmountInOrderCurrency <= 0)
            {
                return Response<int>.Fail(
                    400,
                    "Ödeme alınmamış veya ödemesi tamamlanmammış bir sipariş için fatura oluşturulamaz.");
            }

            var invoicedAmount = order.Invoices?.Where(invoice =>invoice.IsActive &&
                                                        !invoice.IsDeleted)
                                                    .Sum(invoice => invoice.FinalAmount) ?? 0m;

            var invoiceableAmount = paidAmountInOrderCurrency - invoicedAmount; 

            if (invoiceableAmount <= 0)
            {
                return Response<int>.Fail(
                    400,
                    "Faturalanabilir ödeme tutarı bulunmamaktadır.");
            }

            decimal totalAmount = 0;
            decimal totalTax = 0;

            foreach (var item in order.OrderItems)
            {
                var itemSubTotalLocal = ((decimal)item.Quantity * item.UnitPrice) - item.Discount;
                var itemTaxLocal = itemSubTotalLocal * (item.TaxRate / 100m);

                decimal exchangeRate = 1; // varsayılan çarpan (kurlar aynıysa). eğer item in kuru ile faturanın kuru farklıysa dönüştüreceğiz. 

                if (item.Currency != request.Currency)
                {
                    exchangeRate = await _currencyService.GetExchangeRateAsync(fromCurrencyId: item.Currency, toCurrencyId: request.Currency, date: request.InvoiceDate, cancellationToken);
                }

                totalAmount += itemSubTotalLocal * exchangeRate;
                totalTax += itemTaxLocal * exchangeRate;
            }

            // sipariş genel indirimi de faturaya yansıtılmalıdır
            decimal convertedOrderDiscount = order.Discount;

            if (order.Currency != request.Currency && order.Discount > 0)
            {
                var orderExchangeRate =
                    await _currencyService.GetExchangeRateAsync(
                        fromCurrencyId: order.Currency,
                        toCurrencyId: request.Currency,
                        date: request.InvoiceDate,
                        cancellationToken: cancellationToken);

                convertedOrderDiscount = order.Discount * orderExchangeRate;
            }

            decimal finalAmount = totalAmount + totalTax - convertedOrderDiscount;

            if (finalAmount <= 0)
            {
                return Response<int>.Fail(
                    400,
                    "Hesaplanan fatura tutarı sıfırdan büyük olmalıdır.");
            }

            // ödenen tutarı da fatura para birimine dönüştürüyoruz
            decimal paidAmountInInvoiceCurrency = paidAmountInOrderCurrency;

            if (order.Currency != request.Currency)
            {
                var paymentExchangeRate =
                    await _currencyService.GetExchangeRateAsync(
                        fromCurrencyId: order.Currency,
                        toCurrencyId: request.Currency,
                        date: request.InvoiceDate,
                        cancellationToken: cancellationToken);

                paidAmountInInvoiceCurrency = paidAmountInOrderCurrency * paymentExchangeRate;
            }

          
            // tam sipariş faturası oluşturacaksak (şuan iş kuralı böyle) ödeme de tam tutarı karşılamalı
            if (finalAmount > paidAmountInInvoiceCurrency)
            {
                return Response<int>.Fail(
                    400,
                    $"Siparişin tamamı henüz ödenmediği için tam fatura oluşturulamaz. " +
                    $"Fatura tutarı: {finalAmount:N2}, " +
                    $"ödenen tutar: {paidAmountInInvoiceCurrency:N2}");
            }

            string invoiceNumber = await GenerateUniqueInvoiceNumberAsync(cancellationToken);

            var invoice = request.Adapt<Invoice>();
            invoice.InvoiceNumber = invoiceNumber;
            invoice.TotalAmount = totalAmount;
            invoice.TotalTax = totalTax;
            invoice.FinalAmount = finalAmount;

            await _invoiceRepository.AddAsync(invoice);

            order.InvoicedAmount = order.FinalAmount; // order.InvoicedAmount sipariş para biriminde tutuluyorsa invoice farklı para birimindeyken finalAmount doğrudan yazılmamalıdır.Tam fatura oluşturduğumuz için siparişin kendi finalAmount değerini kullanıyoruz.
            order.CanCreateInvoice = false;

            _orderRepository.Update(order);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return Response<int>.Success(invoice.Id, 201, $"Fatura başarıyla oluşturuldu. Fatura No: {invoiceNumber}");
        }

        // örn: INV-2026-X8B9Q2
        private async Task<string> GenerateUniqueInvoiceNumberAsync(CancellationToken cancellationToken)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string generatedNumber;
            bool isUnique = false;

            // Benzersiz bir numara bulana kadar rastgele üretmeye devam et
            do
            {
                // 6 haneli rastgele alfanümerik kod üret
                var randomString = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                // Yıla ve koda göre birleştir (Örn: INV-2026-A4F98K)
                generatedNumber = $"INV-{DateTime.Now.Year}-{randomString}";

                // Veritabanında bu numara daha önce kullanılmış mı kontrol et
                isUnique = !await _invoiceRepository.AnyAsync(i => i.InvoiceNumber == generatedNumber, cancellationToken);

            } while (!isUnique);

            return generatedNumber;
        }
    }
}

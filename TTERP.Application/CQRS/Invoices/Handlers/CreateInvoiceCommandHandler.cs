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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrencyService _currencyService;

        public CreateInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IOrderRepository orderRepository, IUnitOfWork unitOfWork, ICurrencyService currencyService)
        {
            _invoiceRepository = invoiceRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _currencyService = currencyService;
        }

        public async Task<Response<int>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderWithOrderItemsAsync(request.OrderId, cancellationToken);

            if (order == null)
                return Response<int>.Fail(404, "Faturalandırılmak istenen sipariş bulunamadı.");

            if (order.OrderItems == null)
                return Response<int>.Fail(404, "Faturalandırılmak istenen siparişin kalemleri bulunamadı.");

            decimal totalAmount = 0;
            decimal totalTax = 0;

            foreach (var item in order.OrderItems)
            {
                var itemSubTotalLocal = ((decimal)item.Quantity * item.UnitPrice) - item.Discount;
                var itemTaxLocal = itemSubTotalLocal * (item.TaxRate / 100m);

                decimal exchangeRate = 1; // varsayılan çarpan (kurlar aynıysa). eğer itemin kuru ile faturanın kuru farklıysa dönüştüreceğiz. 

                if (item.Currency != request.Currency)
                {
                    exchangeRate = await _currencyService.GetExchangeRateAsync(fromCurrencyId: item.Currency, toCurrencyId: request.Currency, date: request.InvoiceDate, cancellationToken);
                }

                var convertedSubTotal = itemSubTotalLocal * exchangeRate;
                var convertedTax = itemTaxLocal * exchangeRate;

                totalAmount += convertedSubTotal;
                totalTax += convertedTax;
            }

            decimal finalAmount = totalAmount + totalTax;

            string invoiceNumber = await GenerateUniqueInvoiceNumberAsync(cancellationToken);

            var invoice = request.Adapt<Invoice>();
            invoice.InvoiceNumber = invoiceNumber;
            invoice.TotalAmount = totalAmount;
            invoice.TotalTax = totalTax;
            invoice.FinalAmount = finalAmount;

            await _invoiceRepository.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

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

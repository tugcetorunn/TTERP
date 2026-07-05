using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Payments.Commands
{
    public class CreatePaymentCommand : IRequest<Response<int>>
    {
        public int OrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        public int? PaymentType { get; set; }
        public int? PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        public int Currency { get; set; }
        public string? Note { get; set; }
    }
}

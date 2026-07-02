using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Payments
{
    public class CreatePaymentDTO
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

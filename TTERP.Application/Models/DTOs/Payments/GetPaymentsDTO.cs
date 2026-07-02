using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Payments
{
    public class GetPaymentsDTO
    {
        public int OrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        public int? PaymentType { get; set; }
        public string? PaymentTypeName { get; set; }
        public int? PaymentStatus { get; set; }
        public string? PaymentStatusName { get; set; }
        public decimal Amount { get; set; }
        public int Currency { get; set; }
        public string CurrencyName { get; set; }
        public string? Note { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

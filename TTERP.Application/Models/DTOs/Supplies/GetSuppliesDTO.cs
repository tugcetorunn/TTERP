using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.SupplyItems;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Supplies
{
    public class GetSuppliesDTO
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime SupplyDate { get; set; }
        public string DocumentNumber { get; set; }
        public int? SupplyStatus { get; set; }
        public string? SupplyStatusName { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public ICollection<GetSupplyItemsDTO>? SupplyItems { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

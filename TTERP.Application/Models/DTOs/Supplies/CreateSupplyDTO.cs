using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.SupplyItems;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Supplies
{
    public class CreateSupplyDTO
    {
        public decimal TotalAmount { get; set; }
        public DateTime SupplyDate { get; set; }
        public string DocumentNumber { get; set; }
        public int? SupplyStatus { get; set; }
        public int? EmployeeId { get; set; }
        public int? SupplierId { get; set; }
        public ICollection<CreateSupplyItemDTO> SupplyItems { get; set; }
    }
}

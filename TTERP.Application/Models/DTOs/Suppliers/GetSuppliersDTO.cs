using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.SupplierMaterials;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Suppliers
{
    public class GetSuppliersDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ContactName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public ICollection<GetSupplierMaterialsDTO>? SupplierMaterials { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

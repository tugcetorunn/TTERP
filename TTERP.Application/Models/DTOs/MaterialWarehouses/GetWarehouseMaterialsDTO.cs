using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.MaterialWarehouses
{
    public class GetWarehouseMaterialsDTO
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public double Quantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

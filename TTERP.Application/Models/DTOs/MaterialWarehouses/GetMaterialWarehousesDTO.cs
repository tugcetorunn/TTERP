using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.MaterialWarehouses
{
    public class GetMaterialWarehousesDTO
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialUnit { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseCode { get; set; }
        public double Quantity { get; set; }
        public int ReasonForEntryOrExit { get; set; }
        public string? ReasonForEntryOrExitName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.MaterialWarehouses
{
    public class GetMaterialStockTimelineDTO
    {
        public string RecordType { get; set; } = null!;
        public int RecordId { get; set; }
        public int MaterialId { get; set; }
        public string? MaterialName { get; set; }
        public string? MaterialCode { get; set; }
        public int WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public string? WarehouseCode { get; set; }
        public double Quantity { get; set; }
        public int? ReasonCode { get; set; }
        public string? ReasonName { get; set; }
        public int? MaterialUnit { get; set; }
        public string? MaterialUnitName { get; set; }
        public int? ProductionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsReservation { get; set; }
        public bool IsReleased { get; set; }
    }
}

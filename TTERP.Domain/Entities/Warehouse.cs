using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Warehouse : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public ICollection<SupplyItem>? SupplyItems { get; set; }
        public ICollection<MaterialWarehouse>? MaterialWarehouses { get; set; }
        public ICollection<ProductWarehouse>? ProductWarehouses { get; set; }
        public ICollection<OrderItemWarehouse>? OrderItemWarehouses { get; set; }
        public ICollection<Production>? SourceProductions { get; set; }
        public ICollection<Production>? TargetProductions { get; set; }
    }
}

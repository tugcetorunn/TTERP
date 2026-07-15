using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Country : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public ICollection<City>? Cities { get; set; } = new List<City>();
    }
}

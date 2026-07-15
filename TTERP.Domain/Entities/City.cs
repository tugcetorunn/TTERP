using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class City : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public int CountryId { get; set; }
        public Country? Country { get; set; } = null!;
        public ICollection<Town>? Towns { get; set; } = new List<Town>();
    }
}

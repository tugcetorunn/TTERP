using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class ParameterDefinition : BaseEntity<int>
    {
        public string ParamType { get; set; }
        public string? Description { get; set; }
        public string? DataType { get; set; } // string, int, bool, datetime...
        public int? DefaultValue { get; set; } // default paramCode
        public ICollection<ParameterValue>? ParameterValues { get; set; }
    }
}

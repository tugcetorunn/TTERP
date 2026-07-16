using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class ParameterValue : BaseEntity<int>
    {
        public int ParamCode { get; set; }
        public string ParamValue { get; set; }
        public string? Description { get; set; }
        public string? BadgeColor { get; set; }
        public string? Icon { get; set; }
        public string? CssClass { get; set; }
        public string? ShortCode { get; set; }
        public string? Symbol { get; set; }
        public int? DisplayOrder { get; set; }
        public int LanguageId { get; set; }
        public int ParameterDefinitionId { get; set; }
        public ParameterDefinition? ParameterDefinition { get; set; }
    }
}

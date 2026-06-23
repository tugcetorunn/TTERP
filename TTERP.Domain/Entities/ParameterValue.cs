using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class ParameterValue : BaseAuditEntity
    {
        public string ParamCode { get; set; }
        public string ParamValue { get; set; }
        public string? Description { get; set; }
        public int LanguageId { get; set; }
        public int ParameterDefinitionId { get; set; }
        public ParameterDefinition? ParameterDefinition { get; set; }

    }
}

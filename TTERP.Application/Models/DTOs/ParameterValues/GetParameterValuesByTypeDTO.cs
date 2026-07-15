using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.ParameterValues
{
    public class GetParameterValuesByTypeDTO
    {
        public int Id { get; set; }
        public int ParamCode { get; set; }
        public string ParamValue { get; set; }
        public string? Description { get; set; }
        public string? BadgeColor { get; set; }
        public string? Icon { get; set; }
        public string? CssClass { get; set; }
        public string? ShortCode { get; set; }
        public string? Symbol { get; set; }
        public int? DisplayOrder { get; set; }
    }
}

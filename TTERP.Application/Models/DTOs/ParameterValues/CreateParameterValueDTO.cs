using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.ParameterValues
{
    public class CreateParameterValueDTO
    {
        public string ParamType { get; set; }
        public string ParamCode { get; set; }
        public string ParamValue { get; set; }
        public string? Description { get; set; }
        public int LanguageId { get; set; }
    }
}

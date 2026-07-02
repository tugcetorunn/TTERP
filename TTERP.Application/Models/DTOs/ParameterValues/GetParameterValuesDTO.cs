using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.ParameterValues
{
    public class GetParameterValuesDTO
    {
        public int ParamCode { get; set; }
        public string ParamValue { get; set; }
        public string? Description { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string ParamType { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

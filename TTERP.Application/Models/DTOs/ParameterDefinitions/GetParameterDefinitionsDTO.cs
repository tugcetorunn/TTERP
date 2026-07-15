using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.ParameterValues;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.ParameterDefinitions
{
    public class GetParameterDefinitionsDTO
    {
        public int Id { get; set; }
        public string ParamType { get; set; }
        public string? Description { get; set; }
        public string? DataType { get; set; }
        public int? DefaultValue { get; set; }
        public List<GetParameterValuesDTO>? ParameterValues { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

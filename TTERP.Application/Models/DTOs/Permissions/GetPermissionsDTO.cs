using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.Permissions
{
    public class GetPermissionsDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Module { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}

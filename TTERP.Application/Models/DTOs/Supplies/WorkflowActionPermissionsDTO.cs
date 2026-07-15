using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.Supplies
{
    public class WorkflowActionPermissionsDTO
    {
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanAddItem { get; set; }
        public bool CanCancel { get; set; }
        public bool CanPrint { get; set; }
    }
}

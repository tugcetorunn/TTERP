using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.Supplies
{
    public class AllowedWorkflowTransitionDTO
    {
        public int TargetStatusCode { get; set; }
        public string ActionName { get; set; }
        public string StatusName { get; set; }
        public string? StatusShortCode { get; set; }
        public string ButtonText { get; set; }
        public string? BadgeColor { get; set; }
        public string? Icon { get; set; }
        public bool RequiresConfirmation { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class TeamManager : BaseAuditEntity
    {
        public int TeamId { get; set; }
        public Team? Team { get; set; }
        public int ManagerId { get; set; }
        public Employee? Manager { get; set; }
    }
}

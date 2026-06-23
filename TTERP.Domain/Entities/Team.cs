using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Team : BaseEntity<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Employee>? Members { get; set; }
        public ICollection<TeamManager>? Managers { get; set; }

    }
}

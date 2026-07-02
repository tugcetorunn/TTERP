using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.DTOs.Employees;
using TTERP.Application.Models.DTOs.TeamManagers;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.Teams
{
    public class CreateTeamDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<int>? MemberIds { get; set; }
        public List<int>? ManagerIds { get; set; }
    }
}

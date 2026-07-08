using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.TeamManagers
{
    public class GetTeamManagersDTO
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string? TeamName { get; set; }
        public int ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

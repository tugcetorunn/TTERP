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
    public class GetTeamsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<GetEmployeesDTO>? Members { get; set; }
        public ICollection<GetTeamManagersDTO>? Managers { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

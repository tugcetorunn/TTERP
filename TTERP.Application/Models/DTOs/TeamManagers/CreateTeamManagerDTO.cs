using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.TeamManagers
{
    public class CreateTeamManagerDTO
    {
        public int TeamId { get; set; }
        public int ManagerId { get; set; }
    }
}

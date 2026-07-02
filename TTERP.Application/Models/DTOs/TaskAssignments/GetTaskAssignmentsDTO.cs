using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.TaskAssignments
{
    public class GetTaskAssignmentsDTO
    {
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int Role { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

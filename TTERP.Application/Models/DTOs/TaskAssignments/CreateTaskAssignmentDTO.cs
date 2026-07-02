using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.DTOs.TaskAssignments
{
    public class CreateTaskAssignmentDTO
    {
        public int EmployeeId { get; set; }
        public int Role { get; set; }
    }
}

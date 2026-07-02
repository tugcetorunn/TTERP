using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Employees.Commands
{
    public class CreateEmployeeCommand : IRequest<Response<int>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? ImagePath { get; set; }
        public int? Gender { get; set; }
        public int? MaritalStatus { get; set; }
        public DateTime HireDate { get; set; }
        public int? TitleId { get; set; }
        public int? TeamId { get; set; }
        public decimal? Salary { get; set; }
    }
}

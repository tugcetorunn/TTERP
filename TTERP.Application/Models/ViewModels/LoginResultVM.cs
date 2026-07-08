using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.ViewModels
{
    public class LoginResultVM
    {
        public int Id { get; set; } // employeeId
        public string Token { get; set; }
        public string FullName { get; set; }
        public bool IsPasswordChanged { get; set; }
        public bool IsAdmin { get; set; }
        public List<string> Roles { get; set; }
        public string Message { get; set; } = "";
    }
}

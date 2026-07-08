using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTERP.Application.Models.DTOs.Employees
{
    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        [Compare(nameof(NewPassword), ErrorMessage = "Şifreler aynı değil.")]
        public string NewPasswordAgain { get; set; }
    }
}

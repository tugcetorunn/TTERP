using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Domain.Entities;

namespace TTERP.Application.Models.ViewModels
{
    public class CreateEmployeeResultVM
    {
        public Employee Employee { get; set; }
        public string InitialPassword { get; set; } // RANDOM VERİLEN ŞİFRE
    }
}

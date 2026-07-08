using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.Models.ViewModels;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Authentications.Queries
{
    public class LoginQuery : IRequest<Response<LoginResultVM>>
    {
        [Required(ErrorMessage = "Mail adresi girilmesi zorunludur."), MaxLength(100)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre girilmesi zorunludur."), MinLength(6)]
        public string Password { get; set; }
    }
}

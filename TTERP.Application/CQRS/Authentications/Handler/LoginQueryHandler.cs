using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Authentications.Queries;
using TTERP.Application.Models.ViewModels;
using TTERP.Domain.Entities;
using TTERP.Domain.Interfaces.ServiceInterfaces;
using TTERP.Shared.Models;

namespace TTERP.Application.CQRS.Authentications.Handler
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Response<LoginResultVM>>
    {
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly IJWTService _jwtService;

        public LoginQueryHandler(UserManager<Employee> userManager, SignInManager<Employee> signInManager, IJWTService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<Response<LoginResultVM>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var employee = await _userManager.FindByEmailAsync(request.Email);
            if (employee == null)
            {
                return Response<LoginResultVM>.Fail(404, "Kullanıcı bulunamadı.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(employee, request.Password, false);
            if (!result.Succeeded)
            {
                return Response<LoginResultVM>.Fail(409, "Şifre yanlış");
            }

            var roles = await _userManager.GetRolesAsync(employee);

            var token = _jwtService.GenerateToken(employee, roles);

            var loginVM = new LoginResultVM
            {
                Id = employee.Id,
                Token = token,
                FullName = $"{employee.FirstName} {employee.LastName}",
                Roles = roles.ToList(),
                IsPasswordChanged = employee.IsPasswordChanged
            };

            return Response<LoginResultVM>.Success(loginVM, 200, "Giriş başarılı.");
        }
    }
}

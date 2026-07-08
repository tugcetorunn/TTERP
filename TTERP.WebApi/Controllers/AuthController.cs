using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TTERP.Application.CQRS.Authentications.Queries;
using TTERP.Application.CQRS.Employees.Commands;
using TTERP.Application.Interfaces;
using TTERP.Application.Models.DTOs.Employees;

namespace TTERP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;

        public AuthController(IMediator mediator, IAuthService authService)
        {
            _mediator = mediator;
            _authService = authService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(nameof(CreateEmployee))]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command)
        {
            var result = await _mediator.Send(command);
            Console.WriteLine("Bir istek geldi.");

            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginQuery query)
        {
            var result = await _mediator.Send(query);
            return CreateActionResultInstance(result);
        }

        [HttpPost(nameof(ChangePassword))]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _authService.ChangePasswordAsync(userId!, dto);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Şifre başarıyla değiştirildi.");
        }
    }
}

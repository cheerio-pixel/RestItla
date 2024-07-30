using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RestItla.Application.DTO.User;
using RestItla.Application.Extras.ResultObject;
using RestItla.Application.Interfaces.Services;
using RestItla.Domain.Enum;
using RestItla.WebApi.Extension;

namespace RestItla.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn(LoginDTO dto)
        {
            Result<LoginResponseDTO> result = await _accountService.Login(dto);
            return result.ToActionResult();
        }

        [HttpPost("register/waiter")]
        public async Task<IActionResult> RegisterWaiter(RegisterDTO dto)
        {
            Result<RegisterResponseDTO> result = await _accountService.Register(dto, Role.Waiter);
            return result.ToActionResult();
        }

        [HttpPost("register/admin")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> RegisterAdmin(RegisterDTO dto)
        {
            Result<RegisterResponseDTO> result = await _accountService.Register(dto, Role.Admin);
            return result.ToActionResult();
        }
    }
}
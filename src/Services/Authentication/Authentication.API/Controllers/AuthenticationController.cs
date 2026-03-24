using Authentication.API.Dtos;
using Authentication.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers
{
    [Route("api/auth")]
    //port: 8006
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _service;

        public AuthenticationController(IAuthenticationService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var data = await _service.LoginAsync(request);
               return StatusCode(data.StatusCode, data);
        }

        [HttpPost("register-client")]
        public async Task<IActionResult> RegisterClient([FromBody] ClientRegistrationDto request)
        {
            var data= await _service.RegisterClientAsync(request);
               return StatusCode(data.StatusCode, data);
        }

        [HttpPost("register-employee")]
        public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeRegistrationDto request)
        {
            var data = await _service.RegisterEmployeeAsync(request);
               return StatusCode(data.StatusCode, data);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(int userId, string refreshToken)
        {
            var data = await _service.RefreshTokensAsync(userId, refreshToken);
               return StatusCode(data.StatusCode, data);
        }
    }
}

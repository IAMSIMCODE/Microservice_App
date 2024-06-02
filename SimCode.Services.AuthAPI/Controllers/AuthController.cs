using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimCode.Services.AuthAPI.Models.Dto.Request;
using SimCode.Services.AuthAPI.Models.Dto.Response;
using SimCode.Services.AuthAPI.Services.IService;

namespace SimCode.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApiResponse _response;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _response = new();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegRequestDto regRequest)
        {
            var userReg = await _authService.Register(regRequest);
            if (!string.IsNullOrEmpty(userReg))
            {
                _response.IsSuccess = false;
                _response.Message = userReg;
                _response.StatusCode = "01";
                return BadRequest(_response);
            }
            else { return Ok(_response); }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var loginRes = await _authService.Login(loginRequest);
            if (loginRes.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid username or password";
                _response.StatusCode = "01";
                return BadRequest(_response);
            }
            else { _response.Result = loginRes; return Ok(_response); }
        }

        [HttpPost]
        [Route("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegRequestDto regRequest)
        {
            var assignRole = await _authService.AssignRole(regRequest.Email, regRequest.Role.ToUpper());
            if (!assignRole)
            {
                _response.IsSuccess = false;
                _response.Message = "Error Occured";
                _response.StatusCode = "01";
                return BadRequest(_response);
            }
            else { return Ok(_response); }
        }
    }
}

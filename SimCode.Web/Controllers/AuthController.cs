using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SimCode.Web.Models.Dto.Request;
using SimCode.Web.Models.Dto.Response;
using SimCode.Web.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static SimCode.Web.Utility.StaticDetail;

namespace SimCode.Web.Controllers
{
    public class AuthController(IAuthService authService, ITokenProvider tokenProvider) : Controller
    {
        private readonly IAuthService _authService = authService;
        private readonly ITokenProvider _tokenProvider = tokenProvider;

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            var response = await _authService.LoginAsync(loginRequest);

            if (response != null && response.IsSuccess)
            {
                var responseObj = JsonConvert.DeserializeObject<LoginResponseDto>(response.Result.ToString());  

                await SigninUser(responseObj);

                _tokenProvider.SetToken(responseObj.Token);

                return RedirectToAction("Index", "Home");   
            }
            else
            {
                TempData["error"] = response.Message;

                //ModelState.AddModelError("CustomError", response.Message);
                return View(loginRequest);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = RoleAdmin, Value = RoleAdmin},
                new() {Text = RoleCustomer, Value = RoleCustomer},
                new() {Text = RoleUser, Value = RoleUser},
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegRequestDto regRequest)
        {
            var result = await _authService.RegisterAsync(regRequest);

            if (result != null && result.IsSuccess)
            {
                //Check if the role is empty
                if (string.IsNullOrEmpty(regRequest.Role))
                {
                    regRequest.Role = RoleUser;
                }

                var assignRole = await _authService.AssignRoleAsync(regRequest);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    TempData["error"] = result.Message;
                }
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text = RoleAdmin, Value = RoleAdmin},
                new() {Text = RoleCustomer, Value = RoleCustomer},
                new() {Text = RoleUser, Value = RoleUser},
            };

            ViewBag.RoleList = roleList;
            return View(regRequest);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }


        //Congigure cookie authentication so that Identity will know that a user is logged in 
        private async Task SigninUser(LoginResponseDto loginResponse)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(loginResponse.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, 
                              jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                              jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                              jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));


            identity.AddClaim(new Claim(ClaimTypes.Name,
                           jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(ClaimTypes.Role,
                           jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}

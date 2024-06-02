using Microsoft.AspNetCore.Identity;
using SimCode.Services.AuthAPI.Data;
using SimCode.Services.AuthAPI.Models.Dto.Request;
using SimCode.Services.AuthAPI.Models.Dto.Response;
using SimCode.Services.AuthAPI.Models.User;
using SimCode.Services.AuthAPI.Services.IService;

namespace SimCode.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> Register(RegRequestDto regRequest)
        {
            var res = string.Empty;

            try
            {
                //var userExist = await _userManager.FindByEmailAsync(regRequest.Email);
                //if (userExist != null) { return res = "User Exist"; }

                ApplicationUser user = new()
                {
                    UserName = regRequest.Email,
                    Email = regRequest.Email,
                    NormalizedEmail = regRequest.Email.ToUpper(),
                    Name = regRequest.Name,
                    PhoneNumber = regRequest.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, regRequest.Password);

                if (result.Succeeded)
                {
                    var userToReturn = _context.ApplicationUsers.First(u => u.Email == regRequest.Email);

                    UserDto userDto = new()
                    {
                        Id = userToReturn.Id,
                        Email = userToReturn.Email,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };
                    return res = "";
                }
                else
                {
                    return res = result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                return res = $"Error Occured | {ex.Message}";
            }
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequest.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);  

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            //Get the User roles

            var roles = await _userManager.GetRolesAsync(user);

            //Generate Jwt token
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDto userDto = new()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new()
            {
                User = userDto,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                //!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult() You can use this line if you want to use it sycronoslly
                
                var roles = await _roleManager.RoleExistsAsync(roleName);
                if (!roles)
                {
                    // Create role if it does not exist 
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }
    }
}

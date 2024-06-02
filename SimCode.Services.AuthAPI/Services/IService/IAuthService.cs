using SimCode.Services.AuthAPI.Models.Dto.Request;
using SimCode.Services.AuthAPI.Models.Dto.Response;

namespace SimCode.Services.AuthAPI.Services.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegRequestDto regRequest);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
        Task<bool> AssignRole(string email, string roleName);
    }
}

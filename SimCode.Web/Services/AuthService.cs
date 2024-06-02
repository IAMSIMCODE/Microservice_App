using SimCode.Web.Models.AppResponse;
using SimCode.Web.Models.Dto.Request;
using SimCode.Web.Services.IServices;
using static SimCode.Web.Utility.StaticDetail;

namespace SimCode.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ApiResponse> AssignRoleAsync(RegRequestDto regRequest)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = regRequest,
                ApiUrl = AuthApiBase + "api/auth/AssignRole"
            });
        }

        public async Task<ApiResponse> LoginAsync(LoginRequestDto loginRequest)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = loginRequest,
                ApiUrl = AuthApiBase + "api/auth/login"
            }, withBearer: false);
        }

        public async Task<ApiResponse> RegisterAsync(RegRequestDto regRequest)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = regRequest,
                ApiUrl = AuthApiBase + "api/auth/register"
            },withBearer: false);
        }
    }
}

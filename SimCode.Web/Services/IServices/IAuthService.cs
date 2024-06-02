using SimCode.Web.Models.AppResponse;
using SimCode.Web.Models.Dto.Request;

namespace SimCode.Web.Services.IServices
{
    public interface IAuthService
    {
        Task<ApiResponse> RegisterAsync(RegRequestDto regRequest);
        Task<ApiResponse> AssignRoleAsync(RegRequestDto regRequest);
        Task<ApiResponse> LoginAsync(LoginRequestDto loginRequest);
    }
}

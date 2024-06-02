using SimCode.Web.Models.AppResponse;
using SimCode.Web.Models.Dto.Request;

namespace SimCode.Web.Services.IServices
{
    public interface IBaseService
    {
        Task<ApiResponse> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}

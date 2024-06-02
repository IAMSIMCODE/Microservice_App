using SimCode.Web.Models.AppResponse;
using SimCode.Web.Models.Dto.Request;

namespace SimCode.Web.Services.IServices
{
    public interface ICouponService
    {
        Task<ApiResponse> GetAllCouponAsync();
        Task<ApiResponse> GetCouponByIdAsync(int id);
        Task<ApiResponse> GetCouponAsync(string couponCode);
        Task<ApiResponse> CreateCouponAsync(CouponDto couponDto);
        Task<ApiResponse> UpdateCouponAsync(CouponUpdateDto couponDto);
        Task<ApiResponse> DeleteCouponAsync(int id);
    }
}

using SimCode.Services.EmailApi.Models.AppResponse;
using SimCode.Services.EmailApi.Models.Dto;

namespace SimCode.Services.EmailApi.Services
{
    public interface ICouponService
    {
        Task<ApiResponse> GetAll();
        Task<ApiResponse> GetSingle(int id);
        Task<ApiResponse> GetSingleByCode(string couponCode);
        Task<ApiResponse> CreateCoupon(CouponDto couponDto);
        Task<ApiResponse> UpdateCoupon(CouponUpdateDto couponDto);
        Task<ApiResponse> DeleteCoupon(int id);
    }
}

using SimCode.Web.Models.AppResponse;
using SimCode.Web.Models.Dto.Request;
using SimCode.Web.Services.IServices;
using static SimCode.Web.Utility.StaticDetail;

namespace SimCode.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ApiResponse> GetAllCouponAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                ApiUrl = CouponApiBase + "api/coupon"
            });
        }

        public async Task<ApiResponse> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                ApiUrl = CouponApiBase + "api/coupon/GetByCode/" + couponCode
            });
        }

        public async Task<ApiResponse> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                ApiUrl = CouponApiBase + "api/coupon/" + id
            });
        }

        public async Task<ApiResponse> CreateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = couponDto,
                ApiUrl = CouponApiBase + "api/coupon/"
            });
        }

        public async Task<ApiResponse> UpdateCouponAsync(CouponUpdateDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Data = couponDto,
                ApiUrl = CouponApiBase + "api/coupon/"
            });
        }

        public async Task<ApiResponse> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                ApiUrl = CouponApiBase + "api/coupon/" + id
            });
        }
    }
}

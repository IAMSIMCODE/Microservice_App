using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimCode.Services.EmailApi.Data;
using SimCode.Services.EmailApi.Models;
using SimCode.Services.EmailApi.Models.AppResponse;
using SimCode.Services.EmailApi.Models.Dto;

namespace SimCode.Services.EmailApi.Services
{
    public class CouponService(AppDbContext context, IMapper mapper) : ICouponService
    {
        private readonly AppDbContext _context = context;
        private readonly ApiResponse _response = new();
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse> GetAll()
        {
            try
            {
                var coupons = await _context.Coupons.ToListAsync();
                if (coupons.Count > 0)
                {
                    _response.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
                }
                else { ReturnResponse(false, "Data not found", "01"); }
            }
            catch (Exception ex) { ReturnResponse(false, $"Error Occured: {ex.Message}", ""); }
            return _response;
        }

        public async Task<ApiResponse> GetSingle(int id)
        {
            try
            {
                var coupon = await _context.Coupons.FindAsync(id);
                if (coupon != null)
                {
                    _response.Result = _mapper.Map<CouponDto>(coupon);
                }
                else {ReturnResponse(false, "Data not found", "01");}
            }
            catch (Exception ex) { ReturnResponse(false, $"Error Occured: {ex.Message}", ""); }
            return _response;   
        }

        public async Task<ApiResponse> GetSingleByCode(string couponCode)
        {
            try
            {
                var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.CouponCode.Equals(couponCode, StringComparison.CurrentCultureIgnoreCase));
                if (coupon != null)
                {
                    _response.Result = _mapper.Map<CouponDto>(coupon);
                }
                else { ReturnResponse(false, "Data not found", "01"); }
            }
            catch (Exception) { ReturnResponse(false, "Data not found", ""); }
            return _response;
        }

        public async Task<ApiResponse> CreateCoupon(CouponDto couponDto)
        {
            try
            {
                var copObj = _mapper.Map<Coupon>(couponDto);
                await _context.Coupons.AddAsync(copObj);

                await _context.SaveChangesAsync();
                _response.Result = _mapper.Map<CouponDto>(copObj);
            }
            catch (Exception ex) { ReturnResponse(false, $"Error Occured: {ex.Message}", ""); }
            return _response;
        }

        public async Task<ApiResponse> DeleteCoupon(int id)
        {
            try
            {
                var coupon = await _context.Coupons.FindAsync(id);
                if (coupon != null)
                {
                    _context.Coupons.Remove(coupon);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex) { ReturnResponse(false, $"Error Occured: {ex.Message}", ""); }
            return _response;
        }

        public async Task<ApiResponse> UpdateCoupon(CouponUpdateDto couponDto)
        {
            try
            {
                var copObj = _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Update(copObj);

                await _context.SaveChangesAsync();
                _response.Result = _mapper.Map<CouponDto>(copObj);
            }
            catch (Exception ex) { ReturnResponse(false, $"Error Occured: {ex.Message}", ""); }
            return _response;
        }

        private void ReturnResponse(bool isSuccess, string message, string statusCode)
        {
            _response.IsSuccess = isSuccess;
            _response.Message = message;
            _response.StatusCode = statusCode;
        }
    }
}

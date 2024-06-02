using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimCode.Web.Models.Dto.Request;
using SimCode.Web.Services.IServices;

namespace SimCode.Web.Controllers
{
    public class CouponController(ICouponService couponService) : Controller
    {
        private readonly ICouponService _couponService = couponService;

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto> couponList = [];

            var response = await _couponService.GetAllCouponAsync();
            if (response != null && response.IsSuccess)
            {
                couponList = JsonConvert.DeserializeObject<List<CouponDto>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return View(couponList);
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> CouponCreate(CouponDto coupon) 
		{
			if (ModelState.IsValid)
            {
                var response = await _couponService.CreateCouponAsync(coupon);
                if (response != null && response.IsSuccess) 
                {
                    TempData["success"] = "Coupon created successfully";
                    return RedirectToAction(nameof(CouponIndex)); 
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }
            return View(coupon);
		}

		public async Task<IActionResult> CouponDelete(int couponId)
		{
			var response = await _couponService.GetCouponByIdAsync(couponId);
            if (response != null && response.IsSuccess)
            {
                var coupon = JsonConvert.DeserializeObject<CouponDto>(response.Result.ToString());  
                return View(coupon);
            }
            else
            {
                TempData["error"] = response.Message;
            } 
            return NotFound();
		}

        [HttpPost]
		public async Task<IActionResult> CouponDelete(CouponDto couponDto)
		{
			var response = await _couponService.DeleteCouponAsync(couponDto.CouponId);
			if (response != null && response.IsSuccess)
			{
                TempData["success"] = "Coupon deleted successfully";
                return RedirectToAction(nameof(CouponIndex));
			}
            else
            {
                TempData["error"] = response.Message;
            }
            return View(couponDto);
		}
	}
}
